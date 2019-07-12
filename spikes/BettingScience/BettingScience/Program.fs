open OddsPortalScraper

[<EntryPoint>]
let main argv =
    // crossed odds
    // odds time

    // 1xbet 417
    // asianOdds 476
    // 188bet 56
    // bet365 419
    // betfair 429
    // bwin 2
    // williamhill 15
    // marafon 381
    // winline 456
    // dafabet 147
    // sbobet 75
    OddsPortalScraper.fetchLeagueDataAndSaveToFile basketballID [outHomeAwayID; outOverUnderID; outAsianHandicapID] ("C2416Q6r", 28, "NBA1819.json")
    0
