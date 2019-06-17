module Domain
open Newtonsoft.Json

type MatchScore = {
    [<JsonProperty(PropertyName = "home")>]
    Home : int
    [<JsonProperty(PropertyName = "away")>]
    Away : int
}
type Outcome2Odds = {
    [<JsonProperty(PropertyName = "home")>]
    Home : float32
    [<JsonProperty(PropertyName = "away")>]
    Away : float32
}
type Outcome3Odds = {
    [<JsonProperty(PropertyName = "home")>]
    Home : float32
    [<JsonProperty(PropertyName = "draw")>]
    Draw : float32
    [<JsonProperty(PropertyName = "away")>]
    Away : float32
}
type OutcomeOdds = X2 of Outcome2Odds | X3 of Outcome3Odds
type MatchOdds = {
    [<JsonProperty(PropertyName = "starting")>]
    Starting : OutcomeOdds
    [<JsonProperty(PropertyName = "closing")>]
    Closing : OutcomeOdds
}
type MatchData = {
    [<JsonProperty(PropertyName = "id")>]
    ID : string
    [<JsonProperty(PropertyName = "url")>]
    Url : string
    [<JsonProperty(PropertyName = "score")>]
    Score:MatchScore
    [<JsonProperty(PropertyName = "odds")>]
    Odds:MatchOdds
}
type LeagueData = {
    [<JsonProperty(PropertyName = "id")>]
    ID : string
    [<JsonProperty(PropertyName = "matches")>]
    Matches:MatchData array
}