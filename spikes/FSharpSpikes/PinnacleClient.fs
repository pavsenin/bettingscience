module PinnacleClient

open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Text
open Newtonsoft.Json
open Microsoft.FSharpLu.Json
open PinnacleDomain

let jsonContentType = "application/json"

let getJsonStringAsync (client:HttpClient) (request:string) =
    async {
        let! response = client.GetAsync request |> Async.AwaitTask
        response.EnsureSuccessStatusCode() |> ignore
        return! (response.Content.ReadAsStringAsync() |> Async.AwaitTask)
    }
let postJsonStringAsync (client:HttpClient) (request:string) data =
    async {
        let postData = Compact.serialize data
        let stringContent = new StringContent(postData, Encoding.UTF8, jsonContentType)
        let! response = client.PostAsync(request, stringContent) |> Async.AwaitTask
        response.EnsureSuccessStatusCode() |> ignore
        return! (response.Content.ReadAsStringAsync() |> Async.AwaitTask)
    }

type PinnacleClient(id, pwd, proxyInfo) =
    let baseAddress = "https://api.pinnacle.com/"
    let getBalanceUrl = "v1/client/balance"
    let getSportsUrl = "v2/sports"
    let getLeaguesUrl = "v2/leagues"
    let getOddsUrl = "v1/odds"
    let getFixturesUrl = "v1/fixtures"
    let getBetsUrl = "v3/bets"

    let placeStraightBetUrl = "v2/bets/straight"

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
        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(sprintf "%s:%s" id pwd)))

    let buildRequest url data =
        let values = data |> List.map(fun (k, v) -> sprintf "%s=%s" k v) |> List.toArray
        let valuesString = String.Join("&", values)
        sprintf "%s?%s" url valuesString

    member this.GetBalance() =
        async {
            let! responseJson = getJsonStringAsync httpClient getBalanceUrl
            return Compact.deserialize<BalanceResponse> responseJson
        }
    member this.GetSports() =
        async {
            let! responseJson = getJsonStringAsync httpClient getSportsUrl
            return Compact.deserialize<SportsResponse> responseJson
        }
    member this.GetLeagues data =
        async {
            let request = buildRequest getLeaguesUrl data
            let! responseJson = getJsonStringAsync httpClient request
            return Compact.deserialize<LeaguesResponse> responseJson
        }
    member this.GetOdds data =
        async {
            let request = buildRequest getOddsUrl data
            let! responseJson = getJsonStringAsync httpClient request
            return Compact.deserialize<OddsResponse> responseJson
        }
    member this.GetFixtures data =
        async {
            let request = buildRequest getFixturesUrl data
            let! responseJson = getJsonStringAsync httpClient request
            return Compact.deserialize<FixturesResponse> responseJson
        }
    member this.GetBets data =
        async {
            let request = buildRequest getBetsUrl data
            let! responseJson = getJsonStringAsync httpClient request
            return Compact.deserialize<BetsResponse> responseJson
        }
    member this.PlaceStraightBet data =
        async {
            let! responseJson = postJsonStringAsync httpClient placeStraightBetUrl data
            return Compact.deserialize<PlaceBetResponse> responseJson
        }
