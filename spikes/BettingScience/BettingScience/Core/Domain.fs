module Domain
open Newtonsoft.Json

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
type MatchOdds = {
    [<JsonProperty(PropertyName = "value")>]
    Value : float32 option
    [<JsonProperty(PropertyName = "odds")>]
    Odds : OddsData
}
type MatchOutcomes = {
    [<JsonProperty(PropertyName = "outcomeID")>]
    OutcomeID : string
    [<JsonProperty(PropertyName = "values")>]
    Values : MatchOdds array
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
    [<JsonProperty(PropertyName = "matches")>]
    Matches:MatchData array
}