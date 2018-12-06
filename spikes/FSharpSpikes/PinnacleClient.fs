module PinnacleClient

open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Text
open Newtonsoft.Json
open Microsoft.FSharpLu.Json

let getJsonAsync<'T> (client:HttpClient) (request:string) =
    async {
        let! response = client.GetAsync request |> Async.AwaitTask
        response.EnsureSuccessStatusCode() |> ignore
        let! json = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        return JsonConvert.DeserializeObject<'T>(json)
    }
let postJsonAsync (client:HttpClient) (request:string) data =
    async {
        let postData = Compact.serialize data
        let stringContent = new StringContent(postData, Encoding.UTF8, "application/json")
        let! response = client.PostAsync(request, stringContent) |> Async.AwaitTask
        response.EnsureSuccessStatusCode() |> ignore         // throw if web request failed
        return! (response.Content.ReadAsStringAsync() |> Async.AwaitTask)
    }

type Balance = {
    [<JsonProperty(PropertyName = "availableBalance")>]
    AvailableBalance : decimal
    [<JsonProperty(PropertyName = "outstandingTransactions")>]
    OutstandingTransactions : decimal
    [<JsonProperty(PropertyName = "givenCredit")>]
    GivenCredit : decimal
    [<JsonProperty(PropertyName = "currency")>]
    Currency : string
}

type OddsFormat = DECIMAL
type WinRiskType = Risk | Win
type BetType = Spread | MoneyLine | TotalPoints | TeamTotalPoints
type TeamType = Team1 | Team2 | Draw
type SideType = Over | Under
type PlaceBetResponseStatus = ACCEPTED | PENDING_ACCEPTANCE | PROCESSED_WITH_ERROR
type PlaceBetErrorCode = ALL_BETTING_CLOSED | ALL_LIVE_BETTING_CLOSED | BLOCKED_CLIENT | INVALID_COUNTRY | BLOCKED_BETTING | INVALID_EVENT | ABOVE_MAX_BET_AMOUNT | BELOW_MIN_BET_AMOUNT | OFFLINE_EVENT | INSUFFICIENT_FUNDS | LINE_CHANGED | RED_CARDS_CHANGED | SCORE_CHANGED | TIME_RESTRICTION | PAST_CUTOFFTIME | ABOVE_EVENT_MAX | INVALID_ODDS_FORMAT | LISTED_PITCHERS_SELECTION_ERROR

type PlaceBetRequest = {
    [<JsonProperty(PropertyName = "uniqueRequestId")>]
    UniqueRequestId:Guid;
    [<JsonProperty(PropertyName = "acceptBetterLine")>]
    AcceptBetterLine:bool;
    [<JsonProperty(PropertyName = "customerReference")>] // not required
    CustomerReference:string option;
    [<JsonProperty(PropertyName = "oddsFormat")>]
    OddsFormat:OddsFormat;
    [<JsonProperty(PropertyName = "stake")>]
    Stake:decimal;
    [<JsonProperty(PropertyName = "winRiskStake")>]
    WinRiskType:WinRiskType;
    [<JsonProperty(PropertyName = "sportId")>]
    SportId:int;
    [<JsonProperty(PropertyName = "eventId")>]
    EventId:int64;
    [<JsonProperty(PropertyName = "periodNumber")>]   // This represents the period of the match. For example, for soccer we have: 0 - Game, 1 - 1st Half, 2 - 2nd Half
    PeriodNumber:int;
    [<JsonProperty(PropertyName = "betType")>]
    BetType:BetType;
    [<JsonProperty(PropertyName = "team")>]           // Chosen team type. This is needed only for SPREAD, MONEYLINE and TEAM_TOTAL_POINTS bet types
    TeamType:TeamType option;
    [<JsonProperty(PropertyName = "side")>]           // Chosen side. This is needed only for TOTAL_POINTS and TEAM_TOTAL_POINTS bet type
    SideType:SideType option;
    [<JsonProperty(PropertyName = "lineId")>]
    LineId:int;
    [<JsonProperty(PropertyName = "altLineId")>]      // Alternate line identification. Not required
    AltLineId:int64 option;
    [<JsonProperty(PropertyName = "pitcher1MustStart")>] // Baseball only. Refers to the pitcher for TEAM_TYPE.Team1. This applicable only for MONEYLINE bet type, for all other bet types this has to be TRUE
    Pitcher1MustStart:bool option;
    [<JsonProperty(PropertyName = "pitcher2MustStart")>] // Baseball only. Refers to the pitcher for TEAM_TYPE.Team1. This applicable only for MONEYLINE bet type, for all other bet types this has to be TRUE
    Pitcher2MustStart:bool option;
}

type PlaceBetResponse = {
    [<JsonProperty(PropertyName = "status")>]
    Status:PlaceBetResponseStatus;
    [<JsonProperty(PropertyName = "errorCode")>]               // If Status is PROCESSED_WITH_ERROR, errorCode will be in the response.
    ErrorCode:PlaceBetErrorCode option;
    [<JsonProperty(PropertyName = "betId")>]                   // The bet ID of the new bet. May be empty on failure.
    BetId:int option;
    [<JsonProperty(PropertyName = "uniqueRequestId")>]         // Echo of the uniqueRequestId from the request.
    UniquerequestId:Guid;
    [<JsonProperty(PropertyName = "betterLineWasAccepted")>]   // Whether or not the bet was accepted on the line that changed in favour of client.
    BetterLineWasAccepted:bool;
}

type PinnacleClient(id, pwd, currency, format, proxyInfo) =
    let baseAddress = "https://api.pinnacle.com/"
    let httpClient =
        match proxyInfo with
        | Some (url, user, password:string) ->
            let proxy = WebProxy()
            proxy.Address <- Uri(url)
            proxy.BypassProxyOnLocal <- false
            proxy.UseDefaultCredentials <- false
            proxy.Credentials <- NetworkCredential(user, password)
            let handler = new HttpClientHandler(Proxy = proxy, UseProxy = true)
            let client = new HttpClient(handler, true)
            client.BaseAddress <- Uri(baseAddress)
            client
        | None ->
            new HttpClient(BaseAddress = Uri(baseAddress))
    do httpClient.DefaultRequestHeaders.Authorization <-
        new AuthenticationHeaderValue("Basic",
              Convert.ToBase64String(Encoding.ASCII.GetBytes(sprintf "%s:%s" id pwd)))

    member this.GetBalance() =
        getJsonAsync<Balance> httpClient "v1/client/balance"

    member this.PlaceBet data =
        postJsonAsync httpClient "v2/bets/straight" data
