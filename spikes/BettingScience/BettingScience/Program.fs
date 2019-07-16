open OddsPortalScraper
open Microsoft.FSharpLu.Json
open Domain
open Analytics

[<EntryPoint>]
let main argv =
    // test crossed odds
    // test save/load data
    // test accuracy 1x2, overunder, handicap
    // accuracy by total / handicap
    // calculate accuracy by sports \ leagues \ seasons \ teams \ bookmakers \ outcomes 
    
    //[("C2416Q6r", 28, "NBA1819.json"); ("bwgTBOAd", 28, "NBA1718.json"); ("vBTNUyLS", 29, "NBA1617.json"); ("MmbLsWh8", 29, "NBA1516.json");
    // ("rcgzdfbO", 29, "NBA1415.json"); ("f7RlGfit", 29, "NBA1314.json"); ("0l4l9qpR", 29, "NBA1213.json"); ("lK6Chj6l", 23, "NBA1112.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile basketballID [outHomeAwayID; outOverUnderID; outAsianHandicapID] file
    //)

    //[("YX6FFOQa", 5, "RPL1213.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile soccerID [out1x2ID; outOverUnderID; outAsianHandicapID] file
    //)

    //let state = {
    //    OpeningScore = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
    //    ClosingScore = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
    //}
    let getInitState out bookID =
        if out = OddsPortalScraper.out1x2ID then
            {
                BookID = bookID;
                Opening = AX3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
                Closing = AX3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
            }
        else
            {
                BookID = bookID;
                Opening = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
                Closing = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
            }
    ["../../../data/soccer/rpl/RPL1819.json"; "../../../data/soccer/rpl/RPL1718.json"; "../../../data/soccer/rpl/RPL1617.json";
     "../../../data/soccer/rpl/RPL1516.json"; "../../../data/soccer/rpl/RPL1415.json"; "../../../data/soccer/rpl/RPL1314.json"]
    |> List.iter (fun file ->
        [(*OddsPortalScraper.out1x2ID;*) OddsPortalScraper.outOverUnderID]
        |> List.iter (fun outID ->
            let bookID = marafonID
            let state = getInitState outID bookID
            let result = analyze (outID, Some 0.5f) state file
            printfn "%s %s %A" file outID result
        )
    )
    0
