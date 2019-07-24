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
    // get rid of fetch tracing in code
    // simple models: random, (home, away) / total, ideal
    // calculate accuracy by sports \ leagues \ seasons \ teams \ bookmakers \ outcomes 
    
    //[("MmbLsWh8", 29, "USA", "NBA", Y15, "NBA1516.json"); ("rcgzdfbO", 29, "USA", "NBA", Y14, "NBA1415.json"); ("f7RlGfit", 29, "USA", "NBA", Y13, "NBA1314.json"); ("0l4l9qpR", 29, "USA", "NBA", Y12, "NBA1213.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile (basketballID, basketballDataID) [| HA; OU; AH |] file
    //)
    //[("Ieiv94gB", 5, "UK", "ATP Wimbledon", Y19, "ATPWIM19.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile (tennisID, tennisDataID) [| HA; OU; AH |] file
    //)
    [("r3414Mwe", 58, "USA", "MLB", Y18, "MLB18.json");
    ("bwFloypH", 58, "USA", "MLB", Y17, "MLB17.json");
    ("67blnzDc", 57, "USA", "MLB", Y16, "MLB16.json");
    ("QgQMkPOM", 57, "USA", "MLB", Y15, "MLB15.json");
    ("Y9I8VpDI", 57, "USA", "MLB", Y14, "MLB14.json");
    ("WQgD6nC9", 58, "USA", "MLB", Y13, "MLB13.json");
    ("lt4GysvE", 57, "USA", "MLB", Y12, "MLB12.json")]
    |> List.iter (fun file ->
        OddsPortalScraper.fetchLeagueDataAndSaveToFile (baseballID, baseballDataID) [| HA; OU; AH |] file
    )

    [("Qyo0OIgA", 7, "Germany", "Bundesliga", Y15, "BUN1516.json");
    ("M1VFOdWr", 7, "Germany", "Bundesliga", Y14, "BUN1415.json");
    ("UV4Vlz26", 7, "Germany", "Bundesliga", Y13, "BUN1314.json");
    ("nNYg5pkp", 7, "Germany", "Bundesliga", Y12, "BUN1213.json")]
    //2 bundesliga

    //("GChlCjfd", 7, "Germany", "Bundesliga2", Y18, "BUN21819.json");
    //("fTCHtXML", 7, "Germany", "Bundesliga2", Y17, "BUN21718.json");
    //("KlL5XT59", 7, "Germany", "Bundesliga2", Y16, "BUN21617.json");
    //("zRjVAGwT", 7, "Germany", "Bundesliga2", Y15, "BUN21516.json");
    //("n1GyUI8L", 7, "Germany", "Bundesliga2", Y14, "BUN21415.json");
    //("CxaBVZl4", 7, "Germany", "Bundesliga2", Y13, "BUN21314.json");
    //("zw4KYpw8", 7, "Germany", "Bundesliga2", Y12, "BUN21213.json");

    //3 bundesliga

    //("zwaqzfn3", 8, "Germany", "Liga3", Y18, "GERL31819.json");
    //("rsFUZ8U1", 8, "Germany", "Liga3", Y17, "GERL31718.json");
    //("buK9WmLF", 8, "Germany", "Liga3", Y16, "GERL31617.json");
    //("69s15f1j", 8, "Germany", "Liga3", Y15, "GERL31516.json");
    //("rHJOMz1e", 8, "Germany", "Liga3", Y14, "GERL31415.json");
    //("OdIUCQv3", 8, "Germany", "Liga3", Y13, "GERL31314.json");
    //("hMrVH44j", 8, "Germany", "Liga3", Y12, "GERL31213.json");

    //Ligue 1

    //("Gji6p9u4", 8, "France", "Ligue1", Y18, "FRAL11819.json");
    //("hn9DAGLG", 8, "France", "Ligue1", Y17, "FRAL11718.json");
    //("OO2KUIR8", 8, "France", "Ligue1", Y16, "FRAL11617.json");
    //("2Ll8xq90", 8, "France", "Ligue1", Y15, "FRAL11516.json");
    //("jLZ9xBGT", 8, "France", "Ligue1", Y14, "FRAL11415.json");
    //("EmMn2XCi", 8, "France", "Ligue1", Y13, "FRAL11314.json");
    //("hUnbv6tG", 8, "France", "Ligue1", Y12, "FRAL11213.json");

    //Lique 2

    //("fsjAqTfA", 8, "France", "Ligue2", Y18, "FRAL21819.json");
    //("YqNsgljq", 8, "France", "Ligue2", Y17, "FRAL21718.json");
    //("I9vuI2cL", 8, "France", "Ligue2", Y16, "FRAL21617.json");
    //("tCmCy3O6", 8, "France", "Ligue2", Y15, "FRAL21516.json");
    //("AkYRdE1o", 8, "France", "Ligue2", Y14, "FRAL21415.json");
    //("n1Kf0Bs4", 8, "France", "Ligue2", Y13, "FRAL21314.json");
    //("zwAAwQeM", 8, "France", "Ligue2", Y12, "FRAL21213.json")]
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
