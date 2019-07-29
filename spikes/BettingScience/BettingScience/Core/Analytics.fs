module Analytics

open Domain
open Utils
open System
open Accord.Controls
open System.IO
open Microsoft.FSharpLu.Json

type PredictionX2 = { PO1 : float32; PO2 : float32 }
type PredictionX3 = { PO1 : float32; PO0 : float32; PO2 : float32 }
type Prediction = PX2 of PredictionX2 | PX3 of PredictionX3
type MatchResult = O1 | O0 | O2
type Accuracy = { Expected : float32; Variance : float32 }
type AccuracyScoreX2 = { AO1 : Accuracy; AO2 : Accuracy }
type AccuracyScoreX3 = { AO1 : Accuracy; AO0 : Accuracy; AO2 : Accuracy }
type AccuracyScore = AX2 of AccuracyScoreX2 | AX3 of AccuracyScoreX3
type BookAccuracyScore = { Book : Bookmaker; Count : int; Opening : AccuracyScore; Closing : AccuracyScore }
type Accuracy with
    member this.ToFullString() = sprintf "(%.2f,%.2f)" this.Expected this.Variance
let normalizeScore score count =
    let norm value = value / float32(count) * 100.f
    match score with
    | AX2 { AO1 = { Expected = o1e; Variance = o1v }; AO2 = { Expected = o2e; Variance = o2v } } ->
        AX2 { AO1 = { Expected = o1e; Variance = norm o1v }; AO2 = { Expected = o2e; Variance = norm o2v } }
    | AX3 { AO1 = { Expected = o1e; Variance = o1v }; AO0 = { Expected = o0e; Variance = o0v }; AO2 = { Expected = o2e; Variance = o2v } } ->
        AX3 { AO1 = { Expected = o1e; Variance = norm o1v }; AO0 = { Expected = o0e; Variance = norm o0v }; AO2 = { Expected = o2e; Variance = norm o2v } }
let toStringScore score =
    match score with
    | AX2 { AO1 = o1; AO2 = o2 } -> sprintf "O1%s; O2%s" (o1.ToFullString()) (o2.ToFullString())
    | AX3 { AO1 = o1; AO0 = o0; AO2 = o2 } -> sprintf "O1%s; O0%s; O2%s" (o1.ToFullString()) (o0.ToFullString()) (o2.ToFullString())
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
let getProbabilities odds  =
    match odds with
    | X3 { O1 = (o1, _); O0 = (o0, _); O2 = (o2, _) } ->
        let o1Prob, o0Prob, o2Prob = 1.f / o1, 1.f / o0, 1.f / o2
        let sumProb = o1Prob + o0Prob + o2Prob
        PX3 { PO1 = o1Prob / sumProb; PO0 = o0Prob / sumProb; PO2 = o2Prob / sumProb }
    | X2 { O1 = (o1, _); O2 = (o2, _) } ->
        let o1Prob, o2Prob = 1.f / o1, 1.f / o2
        let sumProb = o1Prob + o2Prob
        PX2 { PO1 = o1Prob / sumProb; PO2 = o2Prob / sumProb }
let getAccuracy accuracy probabilities real =
    match probabilities, real, accuracy with
    | PX2 { PO1 = o1; PO2 = o2 },
      PX2 { PO1 = ro1; PO2 = ro2 },
      AX2 { AO1 = { Expected = o1e; Variance = o1v }; AO2 = { Expected = o2e; Variance = o2v } } ->
        let diffO1, diffO2 = ro1 - o1, ro2 - o2
        AX2 { AO1 = { Expected = round(o1e + diffO1); Variance = round(o1v + diffO1 * diffO1) };
             AO2 = { Expected = round(o2e + diffO2); Variance = round(o2v + diffO2 * diffO2) } }
    | PX3 { PO1 = o1; PO0 = o0; PO2 = o2 },
      PX3 { PO1 = ro1; PO0 = ro0; PO2 = ro2 },
      AX3 { AO1 = { Expected = o1e; Variance = o1v }; AO0 = { Expected = o0e; Variance = o0v }; AO2 = { Expected = o2e; Variance = o2v } } ->
        let diffO1, diffO0, diffO2 = ro1 - o1, ro0 - o0, ro2 - o2
        AX3 { AO1 = { Expected = round(o1e + diffO1); Variance = round(o1v + diffO1 * diffO1) };
             AO0 = { Expected = round(o0e + diffO0); Variance = round(o0v + diffO0 * diffO0) };
             AO2 = { Expected = round(o2e + diffO2); Variance = round(o2v + diffO2 * diffO2) } }
    | _ -> accuracy
let getReal score out handicap =
    match out with
    | O1X2 ->
        let result = getMatchResult score
        if result = O1 then Some(PX3 { PO1 = 1.f; PO0 = 0.f; PO2 = 0.f })
        else if result = O0 then Some(PX3 { PO1 = 0.f; PO0 = 1.f; PO2 = 0.f })
        else Some(PX3 { PO1 = 0.f; PO0 = 0.f; PO2 = 1.f })
    | HA ->
        let result = getMatchResult score
        if result = O1 then Some (PX2 { PO1 = 1.f; PO2 = 0.f }) else Some (PX2 { PO1 = 0.f; PO2 = 1.f })
    | OU ->
        handicap ||> (fun handicap ->
            let total = float32(getMatchTotal score)
            if total > handicap then Some(PX2 { PO1 = 1.f; PO2 = 0.f })
            else if total = handicap then None
            else Some(PX2 { PO1 = 0.f; PO2 = 1.f })
        )
    | AH ->
        handicap ||> (fun handicap ->
            let result = getMatchResultWithHandicap score handicap
            match result with
            | O0 -> None
            | O1 -> Some(PX2 { PO1 = 1.f; PO2 = 0.f })
            | O2 -> Some(PX2 { PO1 = 0.f; PO2 = 1.f })
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
            book |>> (fun book ->

                compute state score out handicap book
            )
        ) |> defArg state
    ) state

let check matches =
    let inputs, outputs =
        matches
        |> Array.choose (fun m ->
            let outcome = m.Odds |> Array.tryFind (fun o -> o.Outcome = HA)
            let value = outcome ||> (fun out -> out.Values |> Array.tryFind (fun v -> v.Value = None))
            let odds = value ||> (fun v -> v.BookOdds |> Array.tryFind (fun b -> b.Book = Pin)) |>> (fun b -> b.Odds)
            match odds with
            |  Some { Opening = X2 { O1 = (coefoO1, _); O2 = (coefoO2, _) } as opening;
                      Closing = X2 { O1 = (coefcO1, _); O2 = (coefcO2, _) } as closing } ->
                let result = getMatchResult m.Score
                let real = if result = O1 then 1 else 0
                let openingProb = getProbabilities opening
                let closingProb = getProbabilities closing
                match openingProb, closingProb with
                | PX2 { PO1 = oO1; PO2 = oO2 }, PX2 { PO1 = cO1; PO2 = cO2 } ->
                    let diff = (cO1 - oO1) / oO1
                    //printfn "%f %f %d" oO1 diff real
                    if diff > 2.f then None
                    else if cO1 > 0.35f then None
                    else
                        //printfn "CoefcO1 %f Real %d Match %s" coefcO1 real m.Url
                        Some([|double(cO1); double(diff)|], real)
                | _ -> None
            | _ -> None
        ) |> Array.unzip
        //|> Array.groupBy (fun (prob, _) -> prob)
        //|> Array.sortBy (fun (prob, _) -> prob)
        //|> Array.filter (fun (_, arr) -> (Array.length arr) >= 5)
        //|> Array.map (fun (prob, arr) -> (prob, arr |> Array.map (fun (_, r) -> r) |> Array.average))
        //|> Array.map (fun (prob, real) -> [|double(prob); double(real)|])
    ScatterplotBox.Show("MLB", inputs, outputs).SetSymbolSize(2.f).Hold()

let machineLearning() =
    let getMatches directory trainSeasons validationSeasons =
        let baseDirectory = AppDomain.CurrentDomain.BaseDirectory
        let files = Directory.GetFiles(Path.Combine(baseDirectory, directory), "*.json", SearchOption.AllDirectories)
        let leagues = files |> Array.map (fun file -> Compact.deserializeFile<LeagueData> file)
        let train, valid =
            leagues |> Array.fold(fun (train, valid) l ->
                let contains seasons value = seasons |> List.contains value
                if contains trainSeasons l.Season then l::train, valid
                else if contains validationSeasons l.Season then train, l::valid
                else train, valid
            ) ([], [])
        let collectMatches leagues =
            leagues |> List.map (fun l -> l.Matches) |> Array.concat |> Array.sortBy (fun m -> m.Time)
        collectMatches train, collectMatches valid
    let getBookCoefficients getCoef out matchData =
        matchData.Odds |> Array.tryFind (fun o -> o.Outcome = out)
        ||> (fun out ->
            out.Values |> Array.tryFind (fun v -> v.Value = None)
            ||> (fun value ->
                value.BookOdds |> Array.tryFind (fun book -> book.Book = Pin)
                |>> (fun book -> getCoef book.Odds)
            )
        )
    let getClosing = getBookCoefficients (fun odds -> odds.Closing)
    let getOpening = getBookCoefficients (fun odds -> odds.Opening)
    let getBet closing prediction =
        match closing, prediction with
        | Some(X3 { O1 = (c1, _); O0 = (c0, _); O2 = (c2, _) }), Some(PX3 { PO1 = p1; PO0 = p0; PO2 = p2 }) ->
            let m1, m0, m2 = c1 * p1, c0 * p0, c2 * p2
            if m1 > m0 && m1 > m2 then Some(O1, c1)
            else if m0 > m1 && m0 > m2 then Some(O0, c0)
            else Some(O2, c2)
        | Some(X2 { O1 = (c1, _); O2 = (c2, _) }), Some(PX2 { PO1 = p1; PO2 = p2 }) ->
            let m1, m2 = c1 * p1, c2 * p2
            if m1 > m2 then Some(O1, c1) else Some(O2, c2)
        | _ -> None
    let getBookClosingModel out matchData =
        let closing = getClosing out matchData
        closing |>> getProbabilities
    let getBookOpeningModel out matchData =
        let opening = getOpening out matchData
        opening |>> getProbabilities
    let trainDistibutionModel out (set:MatchData array) =
        match out with
        | O1X2 ->
            let o1, o0, o2 =
                set
                |> Array.fold (fun (o1, o0, o2) m ->
                    match getMatchResult m.Score with
                    | O1 -> o1 + 1.f, o0, o2
                    | O0 -> o1, o0 + 1.f, o2
                    | O2 -> o1, o0, o2 + 1.f
                ) (0.f, 0.f, 0.f)
            let total = o1 + o0 + o2
            (fun _ _ -> Some(PX3 { PO1 = o1 / total; PO0 = o0 / total; PO2 = o2 / total }))
        | HA ->
            let o1, o2 =
                set
                |> Array.fold (fun (o1, o2) m ->
                    let closing = getClosing out m
                    match closing, getMatchResult m.Score with
                    | Some _, O1 -> o1 + 1.f, o2
                    | Some _, O2 -> o1, o2 + 1.f
                    | _ -> o1, o2
                ) (0.f, 0.f)
            let total = o1 + o2
            (fun _ _ -> Some(PX2 { PO1 = o1 / total; PO2 = o2 / total }))
        | _ -> failwith "TODO"
    // >>> Models <<<
    let plainModel out =
        match out with
        | O1X2 ->
            let of3 = 1.f / 3.f;
            Some(PX3 { PO1 = of3; PO0 = of3; PO2 = of3 })
        | _ ->
            Some(PX2 { PO1 = 0.5f; PO2 = 0.5f })
    let idealModel out matchData =
        match out with
        | O1X2 ->
            let pred =
                match getMatchResult matchData.Score with
                | O1 -> PX3 { PO1 = 1.f; PO0 = 0.f; PO2 = 0.f }
                | O0 -> PX3 { PO1 = 0.f; PO0 = 1.f; PO2 = 0.f }
                | O2 -> PX3 { PO1 = 0.f; PO0 = 0.f; PO2 = 1.f }
            Some(pred)
        | HA ->
            match getMatchResult matchData.Score with
            | O1 -> Some(PX2 { PO1 = 1.f; PO2 = 0.f })
            | O2 -> Some(PX2 { PO1 = 0.f; PO2 = 1.f })
            | _ -> None
        | _ -> failwith "TODO"
    // >>> <<<
    let trainMatches, validationMatches = getMatches "../../../data/baseball/mlb/" [] [Y12;Y13;Y14;Y15;Y16;Y17;Y18] //[Y12;Y13;Y14;Y15;Y16;Y17] [Y18]
    let out, handicap = HA, None
    let distributionModel = trainDistibutionModel out validationMatches
    let initState = AX2 { AO1 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } }
    let earn, accuracy, count, co1, co2 =
        validationMatches
        |> Array.fold (fun ((earn, accuracy, count, co1, co2) as state) matchData ->
            let closing = getClosing out matchData
            //let pred = plainModel out
            let pred = getBookClosingModel out matchData
            //let pred = getBookOpeningModel out matchData
            //let pred = idealModel out matchData
            //let pred = distributionModel out matchData
            let bet = getBet closing pred
            let result = getMatchResult matchData.Score
            let real = getReal matchData.Score out handicap
            match real, pred, bet, result with
            | Some real, Some pred, Some bet, O1
            | Some real, Some pred, Some bet, O2 ->
                let nco1, nco2 =
                    match bet with
                    | (O1, _) -> co1 + 1, co2
                    | (O2, _) -> co1, co2 + 1
                    | _ -> co1, co2
                let win =
                    match bet, result with
                    | (O1, c), O1
                    //| (O0, c), O0
                    | (O2, c), O2 -> c - 1.f
                    | _ -> -1.f
                System.Diagnostics.Debug.WriteLine(sprintf "%A %A %2f %2f %s" bet result win earn matchData.Url)
                earn + win, getAccuracy accuracy pred real, count + 1, nco1, nco2
            | _ -> state
        ) (0.f, initState, 0, 0, 0)
    let score = normalizeScore accuracy count
    System.Diagnostics.Debug.WriteLine(sprintf "%A" score)