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
    let getMatches directory trainSeasons testSeasons =
        let baseDirectory = AppDomain.CurrentDomain.BaseDirectory
        let files = Directory.GetFiles(Path.Combine(baseDirectory, directory), "*.json", SearchOption.AllDirectories)
        let leagues = files |> Array.map (fun file -> Compact.deserializeFile<LeagueData> file)
        let train, test =
            leagues |> Array.fold(fun (train, test) l ->
                let contains seasons value = seasons |> List.contains value
                if contains trainSeasons l.Season then l::train, test
                else if contains testSeasons l.Season then train, l::test
                else train, test
            ) ([], [])
        let collectMatches leagues =
            leagues |> List.map (fun l -> l.Matches) |> Array.concat |> Array.sortBy (fun m -> m.Time)
        collectMatches train, collectMatches test
    let plainModel out : Prediction option=
        match out with
        | O1X2 ->
            let of3 = 1.f / 3.f;
            Some(X3 { O1 = of3; O0 = of3; O2 = of3 })
        | _ ->
            Some(X2 { O1 = 0.5f; O2 = 0.5f })
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
        let distibutionModel out :Prediction option =
            match out with
            | O1X2 -> Some(X3 { O1 = o1 / total; O0 = o0 / total; O2 = o2 / total })
            | _ -> failwith "TODO"
        distibutionModel
    [<Test>]
    member this.PredictRPL18BasedOnRPL1217PlainModel() =
        let getMatches directory trainSeasons testSeasons =
            let baseDirectory = AppDomain.CurrentDomain.BaseDirectory
            let files = Directory.GetFiles(Path.Combine(baseDirectory, directory), "*.json", SearchOption.AllDirectories)
            let leagues = files |> Array.map (fun file -> Compact.deserializeFile<LeagueData> file)
            let train, test =
                leagues |> Array.fold(fun (train, test) l ->
                    let contains seasons value = seasons |> List.contains value
                    if contains trainSeasons l.Season then l::train, test
                    else if contains testSeasons l.Season then train, l::test
                    else train, test
                ) ([], [])
            let collectMatches leagues =
                leagues |> List.map (fun l -> l.Matches) |> Array.concat |> Array.sortBy (fun m -> m.Time)
            collectMatches train, collectMatches test
        let trainMatches, testMatches = getMatches "../../../data/soccer/england/apl/" [] [Y12;Y13;Y14;Y15;Y16;Y17;Y18]
        let getClosing out m =
            m.Odds |> Array.tryFind (fun o -> o.Outcome = out)
            ||> (fun out ->
                out.Values |> Array.tryFind (fun v -> v.Value = None)
                ||> (fun value ->
                    value.BookOdds |> Array.tryFind (fun book -> book.Book = Pin)
                    |>> (fun book -> book.Odds.Closing)
                )
            )
        let getBookClosingModel out m =
            let closing = getClosing out m
            closing |>> getProbabilities
        //let distibutionModel = trainDistibutionModel trainMatches
        let distibutionModel = trainDistibutionModel testMatches
        let idealModel out matchData =
            match out with
            | O1X2 ->
                let pred:Prediction =
                    match getMatchResult matchData.Score with
                    | O1 -> X3 { O1 = 1.f; O0 = 0.f; O2 = 0.f }
                    | O0 -> X3 { O1 = 0.f; O0 = 1.f; O2 = 0.f }
                    | O2 -> X3 { O1 = 0.f; O0 = 0.f; O2 = 1.f }
                Some(pred)
            | _ -> failwith "TODO"
        let out, handicap = O1X2, None
        let initState:AccuracyScore = X3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
        let earn, accuracy, count =
            testMatches
            |> Array.fold (fun ((earn, accuracy, count) as state) matchData ->
                let closing = getClosing out matchData
                //let pred = plainModel out
                //let pred = getBookClosingModel out matchData
                let pred = idealModel out matchData
                //let pred = distibutionModel out
                let bet =
                    match closing, pred with
                    | Some(X3 { O1 = (c1, _); O0 = (c0, _); O2 = (c2, _) }), Some(X3 { O1 = p1; O0 = p0; O2 = p2 }) ->
                        let m1, m0, m2 = c1 * p1, c0 * p0, c2 * p2
                        if m1 > m0 && m1 > m2 then Some(O1, c1)
                        else if m0 > m1 && m0 > m2 then Some(O0, c0)
                        else Some(O2, c2)
                    | _ -> None
                let result = getMatchResult matchData.Score
                let real = getReal matchData.Score out handicap
                match real, pred, bet with
                | Some real, Some pred, Some bet ->
                    let win =
                        match bet, result with
                        | (O1, c), O1 | (O0, c), O0 | (O2, c), O2 -> c - 1.f
                        | _ -> -1.f
                    System.Diagnostics.Debug.WriteLine(sprintf "%A %A %2f %2f %s" bet result win earn matchData.Url)
                    System.Diagnostics.Debug.Flush()
                    earn + win, getAccuracy accuracy pred real, count + 1
                | _ -> state
            ) (0.f, initState, 0)
        let score = normalizeScore accuracy count
        ()