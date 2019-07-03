open OddsPortalScraper

[<EntryPoint>]
let main argv = 
    fetchLeagueDataAndSaveToFile baseballID [outHomeAwayID; outAsianHandicapID; outOverUnderID] ("r3414Mwe", 58, "MLB18.json")
    0
