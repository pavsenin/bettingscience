open OddsPortalScraper
open Microsoft.FSharpLu.Json
open Domain
open Analytics
open FSharp.Data.Runtime.WorldBank
open System
open Utils
open System.IO

[<EntryPoint>]
let main argv =
    Console.SetWindowSize(160, 60)
    // test save/load data
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


    //[("jytwvQhq", 5, "Russia", "RPL", Y18, "RPL1819.json");
    //("hdM4QuuS", 5, "Russia", "RPL", Y17, "RPL1718.json");
    //("dSBJYVTs", 5, "Russia", "RPL", Y16, "RPL1617.json");
    //("GQkWIAQ7", 5, "Russia", "RPL", Y15, "RPL1516.json");
    //("Kh7n2gWp", 5, "Russia", "RPL", Y14, "RPL1415.json");
    //("ITC1yoVJ", 5, "Russia", "RPL", Y13, "RPL1314.json");
    //("YX6FFOQa", 5, "Russia", "RPL", Y12, "RPL1213.json")]

    //[("zoZ4r7jR", 8, "England", "APL", Y18, "APL1819.json");
    //("UqQ8LECO", 8, "England", "APL", Y17, "APL1718.json");
    //("8Ai8InSt", 8, "England", "APL", Y16, "APL1617.json");
    //("OhnzLqf7", 8, "England", "APL", Y15, "APL1516.json");
    //("hK4hu76a", 8, "England", "APL", Y14, "APL1415.json");
    //("OtIGJDpL", 8, "England", "APL", Y13, "APL1314.json");
    //("t88zoFDC", 8, "England", "APL", Y12, "APL1213.json");

    //("GKpNhNie", 11, "England", "Championship", Y18, "ENGCham1819.json");
    //("Crp25tJq", 11, "England", "Championship", Y17, "ENGCham1718.json");
    //("4rGIVFO5", 11, "England", "Championship", Y16, "ENGCham1617.json");
    //("2qovK39D", 11, "England", "Championship", Y15, "ENGCham1516.json");
    //("CYE9yYa7", 11, "England", "Championship", Y14, "ENGCham1415.json");
    //("QwlQakAC", 11, "England", "Championship", Y13, "ENGCham1314.json");
    //("QgPQWITg", 11, "England", "Championship", Y12, "ENGCham1213.json");

    //("zZrqwoEl", 11, "England", "League One", Y18, "ENGL11819.json");
    //("jgR4MfcI", 11, "England", "League One", Y17, "ENGL11718.json");
    //("xzXfCRNp", 11, "England", "League One", Y16, "ENGL11617.json");
    //("YBcjpmnE", 11, "England", "League One", Y15, "ENGL11516.json");
    //("Q5lCayhE", 11, "England", "League One", Y14, "ENGL11415.json");
    //("nqNeEi8r", 11, "England", "League One", Y13, "ENGL11314.json");
    //("KGOUVxq0", 11, "England", "League One", Y12, "ENGL11213.json");

    //("fue2w5Tf", 11, "England", "League Two", Y18, "ENGL21819.json");
    //("dWR0NzrC", 11, "England", "League Two", Y17, "ENGL21718.json");
    //("j76czVFi", 11, "England", "League Two", Y16, "ENGL21617.json");
    //("C2dfq71K", 11, "England", "League Two", Y15, "ENGL21516.json");
    //("pf1psTym", 11, "England", "League Two", Y14, "ENGL21415.json");
    //("K0mUb9PI", 11, "England", "League Two", Y13, "ENGL21314.json");
    //("b7NYUdb6", 11, "England", "League Two", Y12, "ENGL21213.json")]
    //@
    //[("W4IGRV1A", 8, "Russia", "FNL", Y18, "FNL1819.json");
    //("jNEmsIct", 8, "Russia", "FNL", Y17, "FNL1718.json");
    //("2J5NzQPQ", 8, "Russia", "FNL", Y16, "FNL1617.json");
    //("Y5UxEHVD", 8, "Russia", "FNL", Y15, "FNL1516.json");
    //("xEJxRx9o", 7, "Russia", "FNL", Y14, "FNL1415.json");
    //("tf8TEz96", 7, "Russia", "FNL", Y13, "FNL1314.json");
    //("Mq3Ksu27", 6, "Russia", "FNL", Y12, "FNL1213.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile (soccerID, soccerDataID) [| O1X2; OU; AH |] file
    //)
    

    let getInitState out book =
        match out with
        | O1X2 ->
            {
                Book = book; Count = 0;
                Opening = AX3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
                Closing = AX3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
            }
        | _ ->
            {
                Book = book; Count = 0;
                Opening = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
                Closing = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
            }
    let generateHandicaps min max out = [|min * 2..max * 2|] |> Array.map (fun h -> (out, Some (float32(h) / 2.f)))
    //let tennisOUMin, tennisOUMax = 20, 50
    //let tennisAHMin, tennisAHMax = -12, 12
    //let tennisOUHandicaps = generateHandicaps 20 50 OU
    //let tennisAHHandicaps = generateHandicaps -12 12 AH
    
    let soccerOUHandicaps = generateHandicaps 1 4 OU
    let soccerAHHandicaps = generateHandicaps -3 3 AH
    let files = Directory.GetFiles("../../../data/soccer/", "*.json", SearchOption.AllDirectories)
    files
    |> Array.iter (fun file ->
        [|(O1X2, None)|]
        //soccerOUHandicaps
        |> Array.iter (fun (out, handicap) ->
            [|B365; BF; Pin; Mar|]
            |> Array.iter (fun book ->
                let state = getInitState out book
                let { Country = country; Division = division; Season = season; Matches = matches;} =
                    Compact.deserializeFile<LeagueData> file
                let { Book = book; Count = count; Opening = opening; Closing = closing } =
                    analyze (Soccer, out, handicap) state matches
                let outText = sprintf "%A%s" out (handicap |>> (fun h -> sprintf "(%.1f)" h) |> defArg "")
                printfn "|%-8s|%-18s|%s|%-8s|%-3d|%-10s|%-49s|%-49s|" country division (season.ToFullString()) (book.ToFullString()) count
                    outText (opening.Normalize(count).ToFullString()) (closing.Normalize(count).ToFullString())
            )
        )
    )
    0
