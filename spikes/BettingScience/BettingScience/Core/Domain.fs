module Domain
open Newtonsoft.Json

type Sport = Soccer | Tennis | Basketball | Baseball
type Bookmaker = Pin | BF | B365 | Mar
type Outcome = HA | O1X2 | OU | AH
type Season = Y12 | Y13 | Y14 | Y15 | Y16 | Y17 | Y18 | Y19
type Season with
    member this.ToFullString() =
        match this with | Y12 -> "2012" | Y13 -> "2013" | Y14 -> "2014" | Y15 -> "2015" | Y16 -> "2016" | Y17 -> "2017" | Y18 -> "2018" | Y19 -> "2019"
type Bookmaker with
    member this.ToFullString() =
        match this with | Pin -> "Pinnacle" | Mar -> "Marafon" | BF -> "Betfair" | B365 -> "Bet365"
type MatchScore = {
    [<JsonProperty(PropertyName = "home")>]
    Home : int
    [<JsonProperty(PropertyName = "away")>]
    Away : int
}
type Outcome2Odds = {
    [<JsonProperty(PropertyName = "o1")>]
    O1 : float32 * int
    [<JsonProperty(PropertyName = "o2")>]
    O2 : float32 * int
}
type Outcome3Odds = {
    [<JsonProperty(PropertyName = "o1")>]
    O1 : float32 * int
    [<JsonProperty(PropertyName = "o0")>]
    O0 : float32 * int
    [<JsonProperty(PropertyName = "o2")>]
    O2 : float32 * int
}
type OutcomeOdds = X2 of Outcome2Odds | X3 of Outcome3Odds
type OddsData = {
    [<JsonProperty(PropertyName = "opening")>]
    Opening : OutcomeOdds
    [<JsonProperty(PropertyName = "closing")>]
    Closing : OutcomeOdds
}
type BookmakerOddsData = {
    [<JsonProperty(PropertyName = "book")>]
    Book : Bookmaker
    [<JsonProperty(PropertyName = "odds")>]
    Odds : OddsData
}
type ValueOdds = {
    [<JsonProperty(PropertyName = "value")>]
    Value : float32 option
    [<JsonProperty(PropertyName = "bookOdds")>]
    BookOdds : BookmakerOddsData array
}
type MatchOutcomes = {
    [<JsonProperty(PropertyName = "outcome")>]
    Outcome : Outcome
    [<JsonProperty(PropertyName = "values")>]
    Values : ValueOdds array
}
type MatchData = {
    [<JsonProperty(PropertyName = "id")>]
    ID : string
    [<JsonProperty(PropertyName = "url")>]
    Url : string
    [<JsonProperty(PropertyName = "teamHome")>]
    TeamHome : string
    [<JsonProperty(PropertyName = "teamAway")>]
    TeamAway : string
    [<JsonProperty(PropertyName = "time")>]
    Time : int
    [<JsonProperty(PropertyName = "score")>]
    Score:MatchScore
    [<JsonProperty(PropertyName = "scoreWithoutOT")>]
    ScoreWithoutOT:MatchScore option
    [<JsonProperty(PropertyName = "periods")>]
    Periods:MatchScore array
    [<JsonProperty(PropertyName = "odds")>]
    Odds:MatchOutcomes array
}
type LeagueData = {
    [<JsonProperty(PropertyName = "id")>]
    ID : string
    [<JsonProperty(PropertyName = "country")>]
    Country : string
    [<JsonProperty(PropertyName = "division")>]
    Division : string
    [<JsonProperty(PropertyName = "season")>]
    Season : Season
    [<JsonProperty(PropertyName = "matches")>]
    Matches:MatchData array
}