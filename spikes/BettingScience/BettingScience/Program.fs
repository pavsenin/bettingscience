open OddsPortalScraper
open Microsoft.FSharpLu.Json
open Domain
open Analytics
open System
open Utils
open System.IO

[<EntryPoint>]
let main argv =
    Console.SetWindowSize(160, 60)
    // избавиться от необходимости везде писать типы
    // tennis to 2/3 sets in wimbledon?
    // get rid of long floats?
    // get rid of fetch tracing in code
    // simple models: random, (home, away) / total, ideal
    // можно ли быть в плюсе за счет variance, если expected 0 
    // calculate accuracy by sports \ leagues \ seasons \ teams \ bookmakers \ outcomes 
    
    //[("MmbLsWh8", 29, "USA", "NBA", Y15, "NBA1516.json"); ("rcgzdfbO", 29, "USA", "NBA", Y14, "NBA1415.json"); ("f7RlGfit", 29, "USA", "NBA", Y13, "NBA1314.json"); ("0l4l9qpR", 29, "USA", "NBA", Y12, "NBA1213.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile (basketballID, basketballDataID) [| HA; OU; AH |] file
    //)
    //[("Ieiv94gB", 5, "UK", "ATP Wimbledon", Y19, "ATPWIM19.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile (tennisID, tennisDataID) [| HA; OU; AH |] file
    //)

    //("Crp25tJq", 11, "England", "Championship", Y17, "ENGCham1718.json");
    [
    ("2qovK39D", 11, "England", "Championship", Y15, "ENGCham1516.json");
    ("CYE9yYa7", 11, "England", "Championship", Y14, "ENGCham1415.json");
    ("QwlQakAC", 11, "England", "Championship", Y13, "ENGCham1314.json");
    ("QgPQWITg", 11, "England", "Championship", Y12, "ENGCham1213.json")]
    |> List.iter (fun file ->
        OddsPortalScraper.fetchLeagueDataAndSaveToFile (soccerID, soccerDataID) [| O1X2; OU; AH |] file
    )
    

    let getInitState out book =
        match out with
        | O1X2 ->
            {
                Book = book; Count = 0;
                Opening = X3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
                Closing = X3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
            }
        | _ ->
            {
                Book = book; Count = 0;
                Opening = X2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
                Closing = X2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
            }
    //let generateHandicaps min max out = [|min * 2..max * 2|] |> Array.map (fun h -> (out, Some (float32(h) / 2.f)))
    //let tennisOUHandicaps = generateHandicaps 20 50 OU
    //let tennisAHHandicaps = generateHandicaps -12 12 AH
    
    //let soccerOUHandicaps = generateHandicaps 1 4 OU
    //let soccerAHHandicaps = generateHandicaps -3 3 AH
    //let files = Directory.GetFiles("../../../data/soccer/england/apl/", "*.json", SearchOption.AllDirectories)
    //files
    //|> Array.iter (fun file ->
    //    [|(O1X2, None)|]
    //    //soccerAHHandicaps
    //    |> Array.iter (fun (out, handicap) ->
    //        [|Pin|]
    //        |> Array.iter (fun book ->
    //            let state = getInitState out book
    //            let { Country = country; Division = division; Season = season; Matches = matches;} =
    //                Compact.deserializeFile<LeagueData> file
    //            let filteredMatches = matches |> Array.filter (fun m -> m.TeamAway = "Manchester Utd" || m.TeamHome = "Manchester Utd")
    //            let { Book = book; Count = count; Opening = opening; Closing = closing } =
    //                analyze (Soccer, out, handicap) state filteredMatches
    //            let outText = sprintf "%A%s" out (handicap |>> (fun h -> sprintf "(%.1f)" h) |> defArg "")
    //            let scoreText score = normalizeScore score count |> toStringScore
    //            printfn "|%-8s|%-18s|%s|%-8s|%-3d|%-10s|%-49s|%-49s|" country division (season.ToFullString()) (book.ToFullString()) count
    //                outText (scoreText opening) (scoreText closing)
    //        )
    //    )
    //)
    0
