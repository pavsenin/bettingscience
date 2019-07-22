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
let normalizeScore (score:AccuracyScore) count : AccuracyScore =
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
let getMatchResult ({ Home = home; Away = away }:MatchScore) =
    if home > away then O1 else if home = away then O0 else O2

let private getMatchResultWithHandicap ({ Home = home; Away = away }:MatchScore) handicap =
    let homeWithHandicap = float32(home) + handicap
    let awayWithHandicap = float32(away)
    if homeWithHandicap > awayWithHandicap then O1 else if homeWithHandicap = awayWithHandicap then O0 else O2
let private getMatchTotal ({ Home = home; Away = away }:MatchScore) =
    home + away
let private getBook book (books:BookmakerOddsData array) = books |> Array.tryFind (fun b -> b.Book = book)
let private isCorrectHandicap = function | None -> true | Some h -> (int(h * 4.f) % 2) = 0
let getProbabilities (odds:OutcomeOdds) : Prediction =
    match odds with
    | X3 { O1 = (o1, _); O0 = (o0, _); O2 = (o2, _) } ->
        let o1Prob, o0Prob, o2Prob = 1.f / o1, 1.f / o0, 1.f / o2
        let sumProb = o1Prob + o0Prob + o2Prob
        X3 { O1 = o1Prob / sumProb; O0 = o0Prob / sumProb; O2 = o2Prob / sumProb }
    | X2 { O1 = (o1, _); O2 = (o2, _) } ->
        let o1Prob, o2Prob = 1.f / o1, 1.f / o2
        let sumProb = o1Prob + o2Prob
        X2 { O1 = o1Prob / sumProb; O2 = o2Prob / sumProb }
let getAccuracy (accuracy:AccuracyScore) (probabilities:Prediction) (real:Prediction) : AccuracyScore =
    match probabilities, real, accuracy with
    | X2 { O1 = o1; O2 = o2 },
      X2 { O1 = ro1; O2 = ro2 },
      X2 { O1 = { Expected = o1e; Variance = o1v }; O2 = { Expected = o2e; Variance = o2v } } ->
        let diffO1, diffO2 = ro1 - o1, ro2 - o2
        X2 { O1 = { Expected = o1e + diffO1; Variance = o1v + diffO1 * diffO1 };
             O2 = { Expected = o2e + diffO2; Variance = o2v + diffO2 * diffO2 } }
    | X3 { O1 = o1; O0 = o0; O2 = o2 },
      X3 { O1 = ro1; O0 = ro0; O2 = ro2 },
      X3 { O1 = { Expected = o1e; Variance = o1v }; O0 = { Expected = o0e; Variance = o0v }; O2 = { Expected = o2e; Variance = o2v } } ->
        let diffO1, diffO0, diffO2 = ro1 - o1, ro0 - o0, ro2 - o2
        X3 { O1 = { Expected = o1e + diffO1; Variance = o1v + diffO1 * diffO1 };
             O0 = { Expected = o0e + diffO0; Variance = o0v + diffO0 * diffO0 };
             O2 = { Expected = o2e + diffO2; Variance = o2v + diffO2 * diffO2 } }
    | _ -> accuracy
let getReal score out handicap :Prediction option =
    match out with
    | O1X2 ->
        let result = getMatchResult score
        if result = O1 then Some(X3 { O1 = 1.f; O0 = 0.f; O2 = 0.f })
        else if result = O0 then Some(X3 { O1 = 0.f; O0 = 1.f; O2 = 0.f })
        else Some(X3 { O1 = 0.f; O0 = 0.f; O2 = 1.f })
    | HA ->
        let result = getMatchResult score
        if result = O1 then Some (X2 { O1 = 1.f; O2 = 0.f }) else Some (X2 { O1 = 0.f; O2 = 1.f })
    | OU ->
        handicap ||> (fun handicap ->
            let total = float32(getMatchTotal score)
            if total > handicap then Some(X2 { O1 = 1.f; O2 = 0.f })
            else if total = handicap then None
            else Some(X2 { O1 = 0.f; O2 = 1.f })
        )
    | AH ->
        handicap ||> (fun handicap ->
            let result = getMatchResultWithHandicap score handicap
            match result with
            | O0 -> None
            | O1 -> Some(X2 { O1 = 1.f; O2 = 0.f })
            | O2 -> Some(X2 { O1 = 0.f; O2 = 1.f })
        )
let compute state score out handicap ({ Odds = { Opening = opening; Closing = closing } }:BookmakerOddsData) =
    let { Book = b; Count = count; Opening = oScore; Closing = cScore } = state
    let real = getReal score out handicap
    match isCorrectHandicap handicap, real with
    | true, Some real ->
        let noScore = getAccuracy oScore (getProbabilities opening) real
        let ncScore = getAccuracy cScore (getProbabilities closing) real
        { Book = b; Count = count + 1; Opening = noScore; Closing = ncScore }
    | _ -> state
    

let analyze (sport, out, ex) state matches =
    matches
    |> Array.fold (fun ({ Book = b } as state:BookAccuracyScore) matchData ->
        let score =
            match sport with
            | Tennis ->
                matchData.Periods |> Array.fold (fun { Home = h; Away = a } p -> { Home = h + p.Home; Away = a + p.Away } ) { Home = 0; Away = 0 }
            | _ -> matchData.Score
        let odd = matchData.Odds |> Array.tryFind (fun o -> o.Outcome = out)
        let value = odd ||> (fun odd -> odd.Values |> Array.tryFind (fun v -> ex = v.Value))
        value ||> (fun { Value = handicap; BookOdds = bookOdds } ->
            let book = getBook b bookOdds
            book |>> (fun book -> compute state score out handicap book)
        ) |> defArg state
    ) state
