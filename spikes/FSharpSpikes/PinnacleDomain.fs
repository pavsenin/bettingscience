module PinnacleDomain

open System
open Newtonsoft.Json

type OddsFormat = DECIMAL
type WinRiskType = RISK | WIN
type BetType = SPREAD | MONEYLINE | TOTALPOINTS | TEAMTOTALPOINTS
type TeamType = TEAM1 | TEAM2 | DRAW
type SideType = OVER | UNDER
type BetListType = SETTLED | RUNNING | ALL
type BetStatus = ACCEPTED | CANCELLED | LOSE | REFUNDED | NOT_ACCEPTED | WON | PENDING_ACCEPTANCE | PROCESSED_WITH_ERROR
type PlaceBetErrorCode =
    ALL_BETTING_CLOSED | ALL_LIVE_BETTING_CLOSED | ABOVE_EVENT_MAX | ABOVE_MAX_BET_AMOUNT | BELOW_MIN_BET_AMOUNT
    | BLOCKED_BETTING | BLOCKED_CLIENT | INSUFFICIENT_FUNDS | INVALID_COUNTRY | INVALID_EVENT | INVALID_ODDS_FORMAT
    | LINE_CHANGED | LISTED_PITCHERS_SELECTION_ERROR | OFFLINE_EVENT | PAST_CUTOFFTIME | RED_CARDS_CHANGED
    | SCORE_CHANGED | TIME_RESTRICTION | DUPLICATE_UNIQUE_REQUEST_ID | INCOMPLETE_CUSTOMER_BETTING_PROFILE
    | INVALID_CUSTOMER_PROFILE | LIMITS_CONFIGURATION_ISSUE | RESPONSIBLE_BETTING_LOSS_LIMIT_EXCEEDED
    | RESPONSIBLE_BETTING_RISK_LIMIT_EXCEEDED | RESUBMIT_REQUEST | SYSTEM_ERROR_3 | LICENCE_RESTRICTION_LIVE_BETTING_BLOCKED
type KeyValue = {
    [<JsonProperty(PropertyName = "key")>]
    Key:string
    [<JsonProperty(PropertyName = "value")>]
    Value:string
}
type CancellationReason = {
    [<JsonProperty(PropertyName = "code")>]
    Code:string
    [<JsonProperty(PropertyName = "details")>]
    Details:KeyValue array
}
type Sport = {
    [<JsonProperty(PropertyName = "id")>]
    Id : int
    [<JsonProperty(PropertyName = "name")>]
    Name : string
    [<JsonProperty(PropertyName = "hasOfferings")>]
    HasOfferings : bool
    [<JsonProperty(PropertyName = "leagueSpecialsCount")>]
    leagueSpecialsCount : int
    [<JsonProperty(PropertyName = "eventSpecialsCount")>]
    eventSpecialsCount : int
    [<JsonProperty(PropertyName = "eventCount")>]
    EventCount : int
}
type SportsResponse = {
    [<JsonProperty(PropertyName = "sports")>]
    Sports : Sport array
}
type StraightBet = {
    [<JsonProperty(PropertyName = "betId")>]
    BetId:int
    [<JsonProperty(PropertyName = "uniqueRequestId")>]
    UniqueRequestId:Guid
    [<JsonProperty(PropertyName = "wagerNumber")>]
    WagerNumber:int
    [<JsonProperty(PropertyName = "placedAt")>]
    PlacedAt:DateTime
    [<JsonProperty(PropertyName = "settledAt")>]
    SettledAt:DateTime option
    [<JsonProperty(PropertyName = "betStatus")>]
    BetStatus:BetStatus
    [<JsonProperty(PropertyName = "betType")>]
    BetType:BetType
    [<JsonProperty(PropertyName = "win")>]
    Win:double
    [<JsonProperty(PropertyName = "risk")>]
    Risk:double
    [<JsonProperty(PropertyName = "oddsFormat")>]
    OddsFormat:OddsFormat
    [<JsonProperty(PropertyName = "updateSequence")>]
    UpdateSequence:int
    [<JsonProperty(PropertyName = "sportId")>]
    SportId:int
    [<JsonProperty(PropertyName = "leagueId")>]
    LeagueId:int
    [<JsonProperty(PropertyName = "eventId")>]
    EventId:int
    [<JsonProperty(PropertyName = "price")>]
    Price:double
    [<JsonProperty(PropertyName = "teamName")>]
    TeamName:string
    [<JsonProperty(PropertyName = "team1")>]
    Team1:string
    [<JsonProperty(PropertyName = "team2")>]
    Team2:string
    [<JsonProperty(PropertyName = "periodNumber")>]
    PeriodNumber:int
    [<JsonProperty(PropertyName = "isLive")>]
    IsLive:bool
    [<JsonProperty(PropertyName = "eventStartTime")>]
    EventStartTime:DateTime
    [<JsonProperty(PropertyName = "winLoss")>]
    WinLoss:double option
    [<JsonProperty(PropertyName = "customerCommission")>]
    CustomerCommission:double option
    [<JsonProperty(PropertyName = "handicap")>]
    Handicap:double option
    [<JsonProperty(PropertyName = "side")>]
    Side:string option
    [<JsonProperty(PropertyName = "pitcher1")>]
    Pitcher1:string option
    [<JsonProperty(PropertyName = "pitcher2")>]
    Pitcher2:string option
    [<JsonProperty(PropertyName = "pitcher1MustStart")>]
    Pitcher1MustStart:bool option
    [<JsonProperty(PropertyName = "pitcher2MustStart")>]
    Pitcher2MustStart:bool option
    [<JsonProperty(PropertyName = "team1Score")>]
    Team1Score:double option
    [<JsonProperty(PropertyName = "team2Score")>]
    Team2Score:double option
    [<JsonProperty(PropertyName = "ftTeam1Score")>]
    FtTeam1Score:double option
    [<JsonProperty(PropertyName = "ftTeam2Score")>]
    FtTeam2Score:double option
    [<JsonProperty(PropertyName = "pTeam1Score")>]
    PTeam1Score:double option
    [<JsonProperty(PropertyName = "pTeam2Score")>]
    PTeam2Score:double option
    [<JsonProperty(PropertyName = "cancellationReason")>]
    CancellationReason:CancellationReason option
}

type BalanceResponse = {
    [<JsonProperty(PropertyName = "availableBalance")>]
    AvailableBalance : double
    [<JsonProperty(PropertyName = "outstandingTransactions")>]
    OutstandingTransactions : double
    [<JsonProperty(PropertyName = "givenCredit")>]
    GivenCredit : double
    [<JsonProperty(PropertyName = "currency")>]
    Currency : string
}
type BetsResponse = {
    [<JsonProperty(PropertyName = "moreAvailable")>]
    MoreAvailable : bool
    [<JsonProperty(PropertyName = "pageSize")>]
    PageSize : int
    [<JsonProperty(PropertyName = "fromRecord")>]
    FromRecord : int
    [<JsonProperty(PropertyName = "toRecord")>]
    ToRecord : int
    [<JsonProperty(PropertyName = "straightBets")>]
    StraightBets : StraightBet array
    [<JsonProperty(PropertyName = "parlayBets")>]
    ParlayBets : obj array
    [<JsonProperty(PropertyName = "teaserBets")>]
    TeaserBets : obj array
    [<JsonProperty(PropertyName = "specialBets")>]
    SpecialBets : obj array
    [<JsonProperty(PropertyName = "manualBets")>]
    ManualBets : obj array
}
type PlaceBetRequest = {
    [<JsonProperty(PropertyName = "uniqueRequestId")>]
    UniqueRequestId:Guid
    [<JsonProperty(PropertyName = "acceptBetterLine")>]
    AcceptBetterLine:bool
    [<JsonProperty(PropertyName = "customerReference")>]
    CustomerReference:string option
    [<JsonProperty(PropertyName = "oddsFormat")>]
    OddsFormat:OddsFormat
    [<JsonProperty(PropertyName = "stake")>]
    Stake:double
    [<JsonProperty(PropertyName = "winRiskStake")>]
    WinRiskType:WinRiskType
    [<JsonProperty(PropertyName = "sportId")>]
    SportId:int
    [<JsonProperty(PropertyName = "eventId")>]
    EventId:int64
    [<JsonProperty(PropertyName = "periodNumber")>]
    PeriodNumber:int
    [<JsonProperty(PropertyName = "betType")>]
    BetType:BetType
    [<JsonProperty(PropertyName = "team")>]
    TeamType:TeamType option
    [<JsonProperty(PropertyName = "side")>]
    SideType:SideType option
    [<JsonProperty(PropertyName = "lineId")>]
    LineId:int
    [<JsonProperty(PropertyName = "altLineId")>]
    AltLineId:int64 option
    [<JsonProperty(PropertyName = "pitcher1MustStart")>]
    Pitcher1MustStart:bool option
    [<JsonProperty(PropertyName = "pitcher2MustStart")>]
    Pitcher2MustStart:bool option
}

type PlaceBetResponse = {
    [<JsonProperty(PropertyName = "status")>]
    Status:BetStatus
    [<JsonProperty(PropertyName = "errorCode")>]
    ErrorCode:PlaceBetErrorCode option
    [<JsonProperty(PropertyName = "uniqueRequestId")>]
    UniqueRequestId:Guid
    [<JsonProperty(PropertyName = "straightBet")>]
    StraightBet:StraightBet option
}