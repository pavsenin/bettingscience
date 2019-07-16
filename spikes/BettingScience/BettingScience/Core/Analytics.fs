﻿module Analytics

open Microsoft.FSharpLu.Json
open Domain
open Utils

type MatchResult = O1 | O0 | O2
type Accuracy = { Expected : float32; Variance : float32 }
type AccuracyScoreX2 = { O1 : Accuracy; O2 : Accuracy }
type AccuracyScoreX3 = { O1 : Accuracy; O0 : Accuracy; O2 : Accuracy }
type AccuracyScore = AX2 of AccuracyScoreX2 | AX3 of AccuracyScoreX3
type BookAccuracyScore = { BookID : string; Opening : AccuracyScore; Closing : AccuracyScore }

let private getProbabilities value = function
    | X3 { O1 = (o1, _); O0 = (o0, _); O2 = (o2, _) } ->
        let o1Prob, o0Prob, o2Prob = 1.f / o1, 1.f / o0, 1.f / o2
        let sumProb = o1Prob + o0Prob + o2Prob
        if value = O1 then o1Prob / sumProb
        else if value = O0 then o2Prob / sumProb
        else o0Prob / sumProb
    | X2 { O1 = (o1, _); O2 = (o2, _) } ->
        let o1Prob, o2Prob = 1.f / o1, 1.f / o2
        let sumProb = o1Prob + o2Prob
        if value = O1 then o1Prob / sumProb
        else o2Prob / sumProb
let private getMatchResult ({ Home = home; Away = away }:MatchScore) = if home > away then O1 else if home = away then O0 else O2
let private getMatchTotal ({ Home = home; Away = away }:MatchScore) = home + away
let private getAccuracy out odds real { Expected = e; Variance = v } =
    let prob = getProbabilities out odds
    let diff = prob - real
    { Expected = e + diff; Variance = v + diff * diff }
let compute state (score, ex) odd =
    match state with
    | { BookID = bookID; Opening = AX2 { O1 = oO1; O2 = oO2 }; Closing = AX2 { O1 = cO1; O2 = cO2 } } ->
        let value = odd.Values |> Array.tryFind (fun v -> ex = v.Value)
        match value with
        | Some { Value = None; BookOdds = bookOdds } ->
            let book = bookOdds |> Array.tryFind (fun b -> b.BookID = bookID)
            match book with
            | Some { Odds = { Opening = opening; Closing = closing } } ->
                let result = getMatchResult score
                let real01, real02 = if result = O1 then 1.f, 0.f else 0.f, 1.f
                let noO1 = getAccuracy O1 opening real01 oO1
                let noO2 = getAccuracy O2 opening real02 oO2
                let ncO1 = getAccuracy O1 closing real01 cO1
                let ncO2 = getAccuracy O2 closing real02 cO2

                { BookID = bookID; Opening = AX2 { O1 = noO1; O2 = noO2 }; Closing = AX2 { O1 = ncO1; O2 = ncO2 } }
            | _ -> state
        | Some { Value = (Some h) as handicap; BookOdds = bookOdds } when handicap = ex ->
            let book = bookOdds |> Array.tryFind (fun b -> b.BookID = bookID)
            match book with
            | Some { Odds = { Opening = opening; Closing = closing } } ->
                let total = getMatchTotal score
                let real01, real02 = if float32(total) > h then 1.f, 0.f else 0.f, 1.f
                let noO1 = getAccuracy O1 opening real01 oO1
                let noO2 = getAccuracy O2 opening real02 oO2
                let ncO1 = getAccuracy O1 closing real01 cO1
                let ncO2 = getAccuracy O2 closing real02 cO2

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
                let real01, realO0, real02 =
                    if result = O1 then 1.f, 0.f, 0.f
                    else if result = O0 then 0.f, 1.f, 0.f
                    else 0.f, 0.f, 1.f
                let noO1 = getAccuracy O1 opening real01 oO1
                let noO0 = getAccuracy O0 opening realO0 oO0
                let noO2 = getAccuracy O2 opening real02 oO2
                let ncO1 = getAccuracy O1 closing real01 cO1
                let ncO0 = getAccuracy O0 closing realO0 cO0
                let ncO2 = getAccuracy O2 closing real02 cO2

                { BookID = bookID; Opening = AX3 { O1 = noO1; O0 = noO0; O2 = noO2 }; Closing = AX3 { O1 = ncO1; O0 = ncO0; O2 = ncO2 } }
            | _ -> state
        | _ -> state
    | _ -> state

let analyze (outID, ex) state fileName =
    let leagueData = Compact.deserializeFile<LeagueData> fileName
    let resultState =
        leagueData.Matches
        |> Array.fold (fun state matchData ->
            matchData.Odds
            |> Array.tryFind (fun out -> out.OutcomeID = outID)
            |>> compute state (matchData.Score, ex)
            |> defArg state
        ) state
    resultState