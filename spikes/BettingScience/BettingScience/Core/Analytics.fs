module Analytics

open Domain
open Utils

type Prediction = XValue<OValue2<float32>, OValue3<float32>>
type MatchResult = O1 | O0 | O2
type Accuracy = { Expected : float32; Variance : float32 }
type AccuracyScoreX2 = OValue2<Accuracy>
type AccuracyScoreX3 = OValue3<Accuracy>
type AccuracyScore = XValue<AccuracyScoreX2, AccuracyScoreX3>
type BookAccuracyScore = { Book : Bookmaker; Count : int; Opening : AccuracyScore; Closing : AccuracyScore }
type Accuracy with
    member this.ToFullString() = sprintf "(%.2f,%.2f)" this.Expected this.Variance
let normalizeScore (score:AccuracyScore) count : AccuracyScore=
    let norm value = value / float32(count) * 100.f
    match score with
    | X2 { O1 = { Expected = o1e; Variance = o1v }; O2 = { Expected = o2e; Variance = o2v } } ->
        X2 { O1 = { Expected = o1e; Variance = norm o1v }; O2 = { Expected = o2e; Variance = norm o2v } }
    | X3 { O1 = { Expected = o1e; Variance = o1v }; O0 = { Expected = o0e; Variance = o0v }; O2 = { Expected = o2e; Variance = o2v } } ->
        X3 { O1 = { Expected = o1e; Variance = norm o1v }; O0 = { Expected = o0e; Variance = norm o0v }; O2 = { Expected = o2e; Variance = norm o2v } }
let toStringScore (score:AccuracyScore) =
    match score with
    | X2 { O1 = o1; O2 = o2 } -> sprintf "O1%s; O2%s" (o1.ToFullString()) (o2.ToFullString())
    | X3 { O1 = o1; O0 = o0; O2 = o2 } -> sprintf "O1%s; O0%s; O2%s" (o1.ToFullString()) (o0.ToFullString()) (o2.ToFullString())

let private getProbabilities (odds:OutcomeOdds) : Prediction =
    match odds with
    | X3 { O1 = (o1, _); O0 = (o0, _); O2 = (o2, _) } ->
        let o1Prob, o0Prob, o2Prob = 1.f / o1, 1.f / o0, 1.f / o2
        let sumProb = o1Prob + o0Prob + o2Prob
        X3 { O1 = o1Prob / sumProb; O0 = o0Prob / sumProb; O2 = o2Prob / sumProb }
    | X2 { O1 = (o1, _); O2 = (o2, _) } ->
        let o1Prob, o2Prob = 1.f / o1, 1.f / o2
        let sumProb = o1Prob + o2Prob
        X2 { O1 = o1Prob / sumProb; O2 = o2Prob / sumProb }
let private getMatchResult ({ Home = home; Away = away }:MatchScore) =
    if home > away then O1 else if home = away then O0 else O2
let private getMatchResultWithHandicap ({ Home = home; Away = away }:MatchScore) handicap =
    let homeWithHandicap = float32(home) + handicap
    let awayWithHandicap = float32(away)
    if homeWithHandicap > awayWithHandicap then O1 else if homeWithHandicap = awayWithHandicap then O0 else O2
let private getMatchTotal ({ Home = home; Away = away }:MatchScore) =
    home + away
let private getAccuracy out odds real { Expected = e; Variance = v } =
    let prob =
        match getProbabilities odds with
        | X2 { O1 = o1; O2 = o2 } -> match out with | O1 -> o1 | _ -> o2
        | X3 { O1 = o1; O0 = o0; O2 = o2 } -> match out with | O1 -> o1 | O0 -> o0 | _ -> o2
    let diff = real - prob
    { Expected = e + diff; Variance = v + diff * diff }
//let private getAccuracy2 odds real { Expected = e; Variance = v } =
//    let prob =
//        match getProbabilities odds with
//        | X2 { O1 = o1; O2 = o2 } -> match out with | O1 -> o1 | _ -> o2
//        | X3 { O1 = o1; O0 = o0; O2 = o2 } -> match out with | O1 -> o1 | O0 -> o0 | _ -> o2
//    let diff = real - prob
//    { Expected = e + diff; Variance = v + diff * diff }
let private getBook book (books:BookmakerOddsData array) = books |> Array.tryFind (fun b -> b.Book = book)
let private isCorrectHandicap = function | None -> true | Some h -> (int(h * 4.f) % 2) = 0
let compute state (score, ex) odd =
    match state with
    | { Book = b; Count = count; Opening = X2 { O1 = oO1; O2 = oO2 }; Closing = X2 { O1 = cO1; O2 = cO2 } } ->
        let value = odd.Values |> Array.tryFind (fun v -> ex = v.Value)
        match value with
        | Some { Value = value; BookOdds = bookOdds } ->
            let isCorrect = isCorrectHandicap value
            let book = getBook b bookOdds
            match isCorrect, book with
            | true, Some { Odds = { Opening = opening; Closing = closing } } ->
                let real = 
                    match odd.Outcome with
                    | HA ->
                        let result = getMatchResult score
                        if result = O1 then Some (1.f, 0.f) else Some (0.f, 1.f)
                    | OU ->
                        value ||> (fun handicap ->
                            let total = float32(getMatchTotal score)
                            if total > handicap then Some(1.f, 0.f)
                            else if total = handicap then None
                            else Some(0.f, 1.f)
                        )
                    | AH ->
                        value ||> (fun handicap ->
                            let result = getMatchResultWithHandicap score handicap
                            match result with
                            | O1 -> Some(1.f, 0.f)
                            | O0 -> None
                            | O2 -> Some(0.f, 1.f)
                        )
                    | _ -> None
                match real with
                | Some (real01, real02) ->
                    let noO1 = getAccuracy O1 opening real01 oO1
                    let noO2 = getAccuracy O2 opening real02 oO2
                    let ncO1 = getAccuracy O1 closing real01 cO1
                    let ncO2 = getAccuracy O2 closing real02 cO2

                    let noO1 = getAccuracy O1 opening real01 oO1
                    let noO2 = getAccuracy O2 opening real02 oO2
                    let ncO1 = getAccuracy O1 closing real01 cO1
                    let ncO2 = getAccuracy O2 closing real02 cO2

                    { Book = b; Count = count + 1; Opening = X2 { O1 = noO1; O2 = noO2 }; Closing = X2 { O1 = ncO1; O2 = ncO2 } }
                | _ -> state
            | _ -> state
        | _ -> state
    | { Book = b; Count = count; Opening = X3 { O1 = oO1; O0 = oO0; O2 = oO2 }; Closing = X3 { O1 = cO1; O0 = cO0; O2 = cO2 } } ->
        match odd.Values with
        | [|{ Value = None; BookOdds = bookOdds }|] ->
            let book = getBook b bookOdds
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

                { Book = b; Count = count + 1; Opening = X3 { O1 = noO1; O0 = noO0; O2 = noO2 }; Closing = X3 { O1 = ncO1; O0 = ncO0; O2 = ncO2 } }
            | _ -> state
        | _ -> state
    | _ -> state

let analyze (sport, out, ex) state matches =
    matches
    |> Array.fold (fun state matchData ->
        let score =
            match sport with
            | Tennis ->
                matchData.Periods |> Array.fold (fun { Home = h; Away = a } p -> { Home = h + p.Home; Away = a + p.Away } ) { Home = 0; Away = 0 }
            | _ -> matchData.Score
        matchData.Odds
        |> Array.tryFind (fun o -> o.Outcome = out)
        |>> compute state (score, ex)
        |> defArg state
    ) state
