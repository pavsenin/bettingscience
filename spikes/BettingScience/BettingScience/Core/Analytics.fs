module Analytics

open Microsoft.FSharpLu.Json
open Domain
open Utils
open System

type MatchResult = Home | Draw | Away

let getProbabilities value = function
    | X3 { O1 = o1; O0 = o0; O2 = o2 } ->
        let o1Prob, o0Prob, o2Prob = 1.f / o1, 1.f / o0, 1.f / o2
        let sumProb = o1Prob + o0Prob + o2Prob
        if value = "o1" then o1Prob / sumProb
        else if value = "o2" then o2Prob / sumProb
        else o0Prob / sumProb
    | X2 { O1 = o1; O2 = o2 } ->
        let o1Prob, o2Prob = 1.f / o1, 1.f / o2
        let sumProb = o1Prob + o2Prob
        if value = "o1" then o1Prob / sumProb
        else o2Prob / sumProb
let getProbabilitiesO1 = getProbabilities "o1"
let getProbabilitiesO2 = getProbabilities "o2"

let check fileName =
    let mutable startingSum = 0.f
    let mutable closingSum = 0.f
    let mutable realSum = 0.f

    let computeHomeAway score odd =
        let getMatchResult ({ Home = home; Away = away }:MatchScore) =
            if home > away then Home else if home = away then Draw else Away
        match odd.Values with
        | [|{ Value = None;
                Odds = { Starting = X2 { O1 = s1; O2 = s2 } as starting;
                        Closing = X2 { O1 = c1; O2 = c2 } as closing } }|] ->
            printfn "Score (%A:%A) Starting %A Closing %A" score.Home score.Away s2 c2
            let startingProb = getProbabilitiesO2 starting
            let closingProb = getProbabilitiesO2 closing
            let result = getMatchResult score
            let real = if result = Home then 1.f else 0.f
            let getMoney r o = if result = r then o - 1.f else -1.f
            startingSum <- startingSum + (getMoney Away s2)
            closingSum <- closingSum + (getMoney Away c2)
            //realSum <- realSum + real
            printfn "StartingSum %f, ClosingSum %f" startingSum closingSum
        | _ -> ()

    let computeHandicap score odd =
        let getMatchResult handicap ({ Home = home; Away = away }:MatchScore) =
            if float32(home) + handicap > float32(away) then Home else Away
        let value = odd.Values |> Array.tryFind (fun ({ Value = h }:MatchOdds) -> match h with | Some hv -> hv <> float32(int(hv)) | _ -> false)
        match value with
        | Some { Value = Some handicap;
                Odds = { Starting = X2 { O1 = s1; O2 = s2 } as starting;
                        Closing = X2 { O1 = c1; O2 = c2 } as closing } } ->
            let result = getMatchResult handicap score
            printfn "Score (%A:%A) Starting %A Closing %A %A %A" score.Home score.Away s2 c2 handicap result
            //let startingProb = getProbabilitiesO2 starting
            //let closingProb = getProbabilitiesO2 closing
            //let real = if result = Home then 1.f else 0.f
            let getMoney r o = if result = r then o - 1.f else -1.f
            startingSum <- startingSum + (getMoney Away s2)
            closingSum <- closingSum + (getMoney Away c2)
            //realSum <- realSum + real
            printfn "StartingSum %f, ClosingSum %f" startingSum closingSum
        | _ -> ()

    let leagueData = Compact.deserializeFile<LeagueData> fileName
    let matches = leagueData.Matches

    for m in matches do
        let oddsValue = m.Odds |> Array.tryFind (fun out -> out.OutcomeID = OddsPortalScraper.outHomeAwayID)
        oddsValue |>> computeHomeAway m.Score |> ignore