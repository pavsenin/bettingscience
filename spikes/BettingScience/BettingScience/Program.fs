open OddsPortalScraper
open Microsoft.FSharpLu.Json
open Domain
open Analytics

[<EntryPoint>]
let main argv =
    // test crossed odds
    // test save/load data
    // accuracy test from blocknote
    // accuracy by total / handicap
    // calculate accuracy by sports \ leagues \ seasons \ teams
    
    //[("C2416Q6r", 28, "NBA1819.json"); ("bwgTBOAd", 28, "NBA1718.json"); ("vBTNUyLS", 29, "NBA1617.json"); ("MmbLsWh8", 29, "NBA1516.json");
    // ("rcgzdfbO", 29, "NBA1415.json"); ("f7RlGfit", 29, "NBA1314.json"); ("0l4l9qpR", 29, "NBA1213.json"); ("lK6Chj6l", 23, "NBA1112.json")]
    //|> List.iter (fun file ->
    //    OddsPortalScraper.fetchLeagueDataAndSaveToFile basketballID [outHomeAwayID; outOverUnderID; outAsianHandicapID] file
    //)

    [("YX6FFOQa", 5, "RPL1213.json")]
    |> List.iter (fun file ->
        OddsPortalScraper.fetchLeagueDataAndSaveToFile soccerID [out1x2ID; outOverUnderID; outAsianHandicapID] file
    )

    //let state = {
    //    OpeningScore = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
    //    ClosingScore = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
    //}
    //["RPL1819.json"; "RPL1718.json"; "RPL1617.json"; "RPL1516.json"; "RPL1415.json"; "RPL1314.json"; "RPL1213.json"]
    //|> List.iter (fun file ->
    //    [pinnacleID; betfairID; bet365ID; marafonID]
    //    |> List.iter (fun bookID ->
    //        let state = {
    //            BookID = bookID;
    //            Opening = AX3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
    //            Closing = AX3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
    //        }
    //        let result = analyze OddsPortalScraper.out1x2ID state file
    //        printfn "%s %s %A" file bookID result
    //    )
    //)
    0
