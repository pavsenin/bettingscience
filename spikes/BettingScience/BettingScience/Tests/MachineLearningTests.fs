module MachineLearningTests
open NUnit.Framework
open Domain
open Analytics
open System.IO
open Microsoft.FSharpLu.Json
open System

[<TestFixture>]
type MachineLearningTests() =
    let predictPlainModel out : Prediction =
        match out with
        | O1X2 ->
            let of3 = 1.f / 3.f;
            X3 { O1 = of3; O0 = of3; O2 = of3 }
        | _ ->
            X2 { O1 = 0.5f; O2 = 0.5f }
    let flatBet = 1.f
    [<Test>]
    member this.PredictRPL18BasedOnRPL1217PlainModelByFlat() =
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
        let trainMatches, testMatches = getMatches "../../../data/soccer/russia/rpl/" [Y12;Y13;Y14;Y15;Y16;Y17] [Y18]
        // train
        testMatches
        |> Array.iter (fun m ->
            ()
        )
        ()