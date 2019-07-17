open OddsPortalScraper
open Microsoft.FSharpLu.Json
open Domain
open Analytics

[<EntryPoint>]
let main argv =
    // test crossed odds
    // test save/load data
    // test analytics nba1819
    // get rid of fetch tracing in code
    // test accuracy 1.0, 1.25
    // calculate accuracy by sports \ leagues \ seasons \ teams \ bookmakers \ outcomes 
    
    //[("C2416Q6r", 28, "USA", "NBA", Y18, "NBA1819.json"); ("bwgTBOAd", 28, "NBA1718.json"); ("vBTNUyLS", 29, "NBA1617.json"); ("MmbLsWh8", 29, "NBA1516.json");
    // ("rcgzdfbO", 29, "NBA1415.json"); ("f7RlGfit", 29, "NBA1314.json"); ("0l4l9qpR", 29, "NBA1213.json"); ("lK6Chj6l", 23, "NBA1112.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile basketballID [| HA; OU; AH |] file
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
    //["../../../data/soccer/rpl/RPL1819.json"; "../../../data/soccer/rpl/RPL1718.json"; "../../../data/soccer/rpl/RPL1617.json";
    // "../../../data/soccer/rpl/RPL1516.json"; "../../../data/soccer/rpl/RPL1415.json"; "../../../data/soccer/rpl/RPL1314.json"]
    //|> List.iter (fun file ->
    //    [(*OddsPortalScraper.out1x2ID;*) OU]
    //    |> List.iter (fun out ->
    //        let state = getInitState out Pin
    //        let result = analyze (out, Some 0.5f) state file
    //        printfn "%s %A %A" file out result
    //    )
    //)
    ["../../../data/soccer/ird1/IRD118.json"; "../../../data/soccer/ird1/IRD117.json"; "../../../data/soccer/ird1/IRD116.json";
     "../../../data/soccer/ird1/IRD115.json"; "../../../data/soccer/ird1/IRD114.json"; "../../../data/soccer/ird1/IRD113.json";
     "../../../data/soccer/ird1/IRD112.json"]
    |> List.iter (fun file ->
        [O1X2]
        |> List.iter (fun out ->
            let state = getInitState out Pin
            let result = analyze (out, None) state file
            printfn "%s %A %A" file out result
        )
    )
    //["../../../data/basketball/nba/NBA1819.json"]
    //|> List.iter (fun file ->
    //    [HA]
    //    |> List.iter (fun out ->
    //        let state = getInitState out Pin
    //        let result = analyze (out, None) state file
    //        printfn "%s %A %A" file out result
    //    )
    //)
    0
