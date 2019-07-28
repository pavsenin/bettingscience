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
    // проверить новые лиги
    // get rid of fetch tracing in code
    // simple models: random, (home, away) / total, ideal
    // calculate accuracy by sports \ leagues \ seasons \ teams \ bookmakers \ outcomes 

    // https://www.oddsportal.com/soccer/russia/youth-league-2018-2019/results/
    // https://www.oddsportal.com/soccer/russia/pfl-center-2018-2019/results/
    // https://www.oddsportal.com/soccer/russia/pfl-south-2018-2019/results/
    // https://www.oddsportal.com/soccer/russia/pfl-west-2018-2019/results/
    // https://www.oddsportal.com/soccer/russia/pfl-east-2018-2019/results/
    // https://www.oddsportal.com/soccer/russia/pfl-ural-povolzhye-2018-2019/results/

    //let matches =
    //    [|"../../../data/baseball/mlb/MLB17.json"; "../../../data/baseball/mlb/MLB18.json"|]
    //    |> Array.map (fun fileName ->
    //        let leagueData = Compact.deserializeFile<LeagueData> fileName
    //        leagueData.Matches
    //    ) |> Array.concat
    //check matches

    //[("MmbLsWh8", 29, "USA", "NBA", Y15, "NBA1516.json"); ("rcgzdfbO", 29, "USA", "NBA", Y14, "NBA1415.json"); ("f7RlGfit", 29, "USA", "NBA", Y13, "NBA1314.json"); ("0l4l9qpR", 29, "USA", "NBA", Y12, "NBA1213.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile (basketballID, basketballDataID) [| HA; OU; AH |] file
    //)
    //[("Ieiv94gB", 5, "UK", "ATP Wimbledon", Y19, "ATPWIM19.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile (tennisID, tennisDataID) [| HA; OU; AH |] file
    //)

    [
    ("EubWlaac", 8, "Italy", "Serie A", Y12, "SERA1213.json");

    ("K28bJgeL", 8, "Italy", "Serie B", Y18, "SERB1819.json");
    ("6ufhxjSq", 10, "Italy", "Serie B", Y17, "SERB1718.json");
    ("r1LsqvuO", 10, "Italy", "Serie B", Y16, "SERB1617.json");
    ("foMr7IwS", 10, "Italy", "Serie B", Y15, "SERB1516.json");
    ("WGbJytvd", 10, "Italy", "Serie B", Y14, "SERB1415.json");
    ("G6oDJsAU", 10, "Italy", "Serie B", Y13, "SERB1314.json");
    ("06vj9g8T", 10, "Italy", "Serie B", Y12, "SERB1213.json")]
    |> List.iter (fun file ->
        OddsPortalScraper.fetchLeagueDataAndSaveToFile (soccerID, soccerDataID) [| O1X2; OU; AH |] file
    )
    

    let getInitState out book =
        match out with
        | O1X2 ->
            {
                Book = book; Count = 0;
                Opening = AX3 { AO1 = { Expected = 0.f; Variance = 0.f }; AO0 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } };
                Closing = AX3 { AO1 = { Expected = 0.f; Variance = 0.f }; AO0 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } }
            }
        | _ ->
            {
                Book = book; Count = 0;
                Opening = AX2 { AO1 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } };
                Closing = AX2 { AO1 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } }
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
