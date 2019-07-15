module Analytics

open Microsoft.FSharpLu.Json
open Domain
open Utils
open System
open OddsPortalScraper

type MatchResult = Home | Draw | Away

let getProbabilities value = function
    | X3 { O1 = (o1, _); O0 = (o0, _); O2 = (o2, _) } ->
        let o1Prob, o0Prob, o2Prob = 1.f / o1, 1.f / o0, 1.f / o2
        let sumProb = o1Prob + o0Prob + o2Prob
        if value = Home then o1Prob / sumProb
        else if value = Away then o2Prob / sumProb
        else o0Prob / sumProb
    | X2 { O1 = (o1, _); O2 = (o2, _) } ->
        let o1Prob, o2Prob = 1.f / o1, 1.f / o2
        let sumProb = o1Prob + o2Prob
        if value = Home then o1Prob / sumProb
        else o2Prob / sumProb
let getProbabilitiesO1 = getProbabilities Home
let getProbabilitiesO2 = getProbabilities Away
let getProbabilitiesO0 = getProbabilities Draw

type Accuracy = { Expected : float32; Variance : float32 }
type AccuracyScoreX2 = { O1 : Accuracy; O2 : Accuracy }
type AccuracyScoreX3 = { O1 : Accuracy; O0 : Accuracy; O2 : Accuracy }
type AccuracyScore = AX2 of AccuracyScoreX2 | AX3 of AccuracyScoreX3
type BookAccuracyScore = { BookID : string; Opening : AccuracyScore; Closing : AccuracyScore }

let analyze outID state fileName =
    let getMatchResult ({ Home = home; Away = away }:MatchScore) = if home > away then Home else if home = away then Draw else Away
    let getAccuracy out odds result { Expected = e; Variance = v } =
        let prob = getProbabilities out odds
        let real = if result = out then 1.f else 0.f
        let diff = prob - real
        { Expected = e + diff; Variance = v + diff * diff }
    let compute state score odd =
        match state with
        | { BookID = bookID; Opening = AX2 { O1 = oO1; O2 = oO2 }; Closing = AX2 { O1 = cO1; O2 = cO2 } } ->
            match odd.Values with
            | [|{ Value = None; BookOdds = bookOdds }|] ->
                let book = bookOdds |> Array.tryFind (fun b -> b.BookID = bookID)
                match book with
                | Some { Odds = { Opening = opening; Closing = closing } } ->
                    let result = getMatchResult score
                    let noO1 = getAccuracy Home opening result oO1
                    let noO2 = getAccuracy Away opening result oO2
                    let ncO1 = getAccuracy Home closing result cO1
                    let ncO2 = getAccuracy Away closing result cO2

                    { BookID = bookID; Opening = AX2 { O1 = noO1; O2 = noO2 }; Closing = AX2 { O1 = ncO1; O2 = ncO2 } }
                | _ -> state
            | _ -> state
        | { BookID = bookID; Opening = AX3 { O1 = oO1; O0 = oO0; O2 = oO2 }; Closing = AX3 { O1 = cO1; O0 = cO0; O2 = cO2 } } ->
            match odd.Values with
            | [|{ Value = None; BookOdds = bookOdds }|] ->
                let book = bookOdds |> Array.tryFind (fun b -> b.BookID = bookID)
                match book with
                | Some { Odds = { Opening = opening; Closing = closing } } ->
                    let result = getMatchResult score
                    let noO1 = getAccuracy Home opening result oO1
                    let noO0 = getAccuracy Draw opening result oO0
                    let noO2 = getAccuracy Away opening result oO2
                    let ncO1 = getAccuracy Home closing result cO1
                    let ncO0 = getAccuracy Draw closing result cO0
                    let ncO2 = getAccuracy Away closing result cO2

                    { BookID = bookID; Opening = AX3 { O1 = noO1; O0 = noO0; O2 = noO2 }; Closing = AX3 { O1 = ncO1; O0 = ncO0; O2 = ncO2 } }
                | _ -> state
            | _ -> state
        | _ -> state

    let leagueData = Compact.deserializeFile<LeagueData> fileName
    let resultState =
        leagueData.Matches
        |> Array.fold (fun state matchData ->
            matchData.Odds
            |> Array.tryFind (fun out -> out.OutcomeID = outID)
            |>> compute state matchData.Score
            |> defArg state
        ) state
    resultState