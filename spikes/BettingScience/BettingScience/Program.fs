open OddsPortalScraper
open Microsoft.FSharpLu.Json
open Domain
open Analytics
open FSharp.Data.Runtime.WorldBank
open System
open Utils

[<EntryPoint>]
let main argv =
    Console.SetWindowSize(130, 60)
    // test save/load data
    // tennis to 2/3 sets in wimbledon?
    // get rid of fetch tracing in code
    // simple models: random, (home, away) / total 
    // calculate accuracy by sports \ leagues \ seasons \ teams \ bookmakers \ outcomes 
    
    //[("MmbLsWh8", 29, "USA", "NBA", Y15, "NBA1516.json"); ("rcgzdfbO", 29, "USA", "NBA", Y14, "NBA1415.json"); ("f7RlGfit", 29, "USA", "NBA", Y13, "NBA1314.json"); ("0l4l9qpR", 29, "USA", "NBA", Y12, "NBA1213.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile (basketballID, basketballDataID) [| HA; OU; AH |] file
    //)
    //[("Ieiv94gB", 5, "UK", "ATP Wimbledon", Y19, "ATPWIM19.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile (tennisID, tennisDataID) [| HA; OU; AH |] file
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
    //["../../../data/soccer/ird1/IRD118.json"; "../../../data/soccer/ird1/IRD117.json"; "../../../data/soccer/ird1/IRD116.json";
    // "../../../data/soccer/ird1/IRD115.json"; "../../../data/soccer/ird1/IRD114.json"; "../../../data/soccer/ird1/IRD113.json";
    // "../../../data/soccer/ird1/IRD112.json"]
    //|> List.iter (fun file ->
    //    [O1X2]
    //    |> List.iter (fun out ->
    //        let state = getInitState out Pin
    //        let result = analyze (out, None) state file
    //        printfn "%s %A %A" file out result
    //    )
    //)
    let tennisOUMin, tennisOUMax = 20, 50
    let tennisAHMin, tennisAHMax = -12, 12
    let generateHandicaps min max out = [min * 2..max * 2] |> List.map (fun h -> (out, Some (float32(h) / 2.f)))
    let tennisOUHandicaps = generateHandicaps tennisOUMin tennisOUMax OU
    let tennisAHHandicaps = generateHandicaps tennisAHMin tennisAHMax AH
    ["../../../data/tennis/atpwim/ATPWIM19.json"]
    |> List.iter (fun file ->
        [(HA, None)] @ tennisAHHandicaps @ tennisOUHandicaps
        |> List.iter (fun (out, handicap) ->
            [Pin]
            |> List.iter (fun book ->
                let state = getInitState out book
                let { Country = country; Division = division; Season = season; Matches = matches;} =
                    Compact.deserializeFile<LeagueData> file
                let { Book = book; Count = count; Opening = opening; Closing = closing } =
                    analyze (Tennis, out, handicap) state matches
                let outText = sprintf "%A(%s)" out (handicap |>> (fun h -> sprintf "%.1f" h) |> defArg "-" )
                printfn "|%-8s|%-15s|%s|%-8s|%3d|%-10s|%-35s|%-35s|" country division (season.ToFullString()) (book.ToFullString()) count
                    outText (opening.ToFullString()) (closing.ToFullString())
            )
        )
    )
    0
