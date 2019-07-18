open OddsPortalScraper
open Microsoft.FSharpLu.Json
open Domain
open Analytics

[<EntryPoint>]
let main argv =
    // test crossed odds
    // test save/load data
    // get rid of fetch tracing in code
    // test accuracy 1.0, 1.25
    // calculate accuracy by sports \ leagues \ seasons \ teams \ bookmakers \ outcomes 
    
    //[("bwgTBOAd", 28, "USA", "NBA", Y17, "NBA1718.json")]
    [("vBTNUyLS", 29, "USA", "NBA", Y16, "NBA1617.json")] //; ("MmbLsWh8", 29, "USA", "NBA", Y15, "NBA1516.json");
    // ("rcgzdfbO", 29, "USA", "NBA", Y14, "NBA1415.json"); ("f7RlGfit", 29, "USA", "NBA", Y13, "NBA1314.json"); ("0l4l9qpR", 29, "USA", "NBA", Y12, "NBA1213.json")]
    |> List.iter (fun file ->
        OddsPortalScraper.fetchLeagueDataAndSaveToFile (basketballID, basketballDataID) [| HA; OU; AH |] file
    )
    //[("Ieiv94gB", 5, "UK", "ATP Wimbledon", Y19, "ATPWIM19.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile (tennisID, tennisDataID) [| HA; OU; AH |] file
    //)
    

    let getInitState out book =
        match out with
        | O1X2 ->
            {
                Book = book;
                Opening = AX3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
                Closing = AX3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
            }
        | _ ->
            {
                Book = book;
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
    ["ATPWIM19.json"]
    |> List.iter (fun file ->
        [OU]
        |> List.iter (fun out ->
            let state = getInitState out Pin
            let result = analyze (Tennis, out, Some 36.5f) state file
            printfn "%s %A %A" file out result
        )
    )
    0
