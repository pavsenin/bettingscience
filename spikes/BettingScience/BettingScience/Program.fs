open OddsPortalScraper

[<EntryPoint>]
let main argv = 
    OddsPortalScraper.fetchLeagueDataAndSaveToFile basketballID [outHomeAwayID; outOverUnderID; outAsianHandicapID] ("C2416Q6r", 28, "NBA1819.json")
    0
