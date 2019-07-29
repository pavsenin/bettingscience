module MachineLearningTests
open NUnit.Framework
open Domain
open Analytics
open System.IO
open Microsoft.FSharpLu.Json
open System
open Utils

[<TestFixture>]
type MachineLearningTests() =
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
            let pred:Prediction =
                match getMatchResult matchData.Score with
                | O1 -> PX3 { PO1 = 1.f; PO0 = 0.f; PO2 = 0.f }
                | O0 -> PX3 { PO1 = 0.f; PO0 = 1.f; PO2 = 0.f }
                | O2 -> PX3 { PO1 = 0.f; PO0 = 0.f; PO2 = 1.f }
            Some(pred)
        | _ -> failwith "TODO"
    let getBookClosingModel out matchData =
        let closing = getClosing out matchData
        closing |>> getProbabilities
    let getBookOpeningModel out matchData =
        let opening = getOpening out matchData
        opening |>> getProbabilities
    let trainDistibutionModel (set:MatchData array) =
        let o1, o0, o2 =
            set
            |> Array.fold (fun (o1, o0, o2) m ->
                match getMatchResult m.Score with
                | O1 -> o1 + 1.f, o0, o2
                | O0 -> o1, o0 + 1.f, o2
                | O2 -> o1, o0, o2 + 1.f
            ) (0.f, 0.f, 0.f)
        let total = o1 + o0 + o2
        let distibutionModel out =
            match out with
            | O1X2 -> Some(PX3 { PO1 = o1 / total; PO0 = o0 / total; PO2 = o2 / total })
            | _ -> failwith "TODO"
        distibutionModel
    // >>> <<<
    [<Test>]
    member this.PredictMLB18BasedOnMLB1217() =
        let trainMatches, validationMatches = getMatches "../../../data/baseball/mlb/" [Y12;Y13;Y14;Y15;Y16;Y17] [Y18]
        let out, handicap = HA, None
        let initState = AX2 { AO1 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } }
        let earn, accuracy, count =
            validationMatches
            |> Array.fold (fun ((earn, accuracy, count) as state) matchData ->
                let closing = getClosing out matchData
                //let pred = plainModel out
                //let pred = getBookClosingModel out matchData
                //let pred = getBookOpeningModel out matchData
                //let pred = idealModel out matchData
                //let pred = trainDistibutionModel trainMatches out
                let pred = trainDistibutionModel validationMatches out
                let bet = getBet closing pred
                let result = getMatchResult matchData.Score
                let real = getReal matchData.Score out handicap
                match real, pred, bet with
                | Some real, Some pred, Some bet ->
                    let win =
                        match bet, result with
                        | (O1, c), O1 | (O0, c), O0 | (O2, c), O2 -> c - 1.f
                        | _ -> -1.f
                    System.Diagnostics.Debug.WriteLine(sprintf "%A %A %2f %2f %s" bet result win earn matchData.Url)
                    earn + win, getAccuracy accuracy pred real, count + 1
                | _ -> state
            ) (0.f, initState, 0)
        let score = normalizeScore accuracy count
        ()