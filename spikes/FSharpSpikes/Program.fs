open System
open System.Net
open PinnacleClient
open Microsoft.FSharpLu.Json
open System.Configuration

[<EntryPoint>]
let main argv =
    let appSettings = ConfigurationManager.AppSettings
    let proxyUrl, proxyLogin, proxyPassword = appSettings.["ProxyUrl"], appSettings.["ProxyLogin"], appSettings.["ProxyPassword"]
    let login, password = appSettings.["Login"], appSettings.["Password"]
    ServicePointManager.SecurityProtocol <- SecurityProtocolType.Tls12
    let proxy = Some(proxyUrl, proxyLogin, proxyPassword)
    let client = PinnacleClient(login, password, "RUB", OddsFormat.DECIMAL, proxy)
    let balance = client.GetBalance() |> Async.RunSynchronously
    printfn "%A" balance

    (*
    let request =
        {
            SportId = 29;
            EventId = 916663454L;
            Stake = 100.0m;
            OddsFormat = OddsFormat.DECIMAL;
            AcceptBetterLine = true;
            UniqueRequestId = Guid.NewGuid();
            PeriodNumber = 0;
            WinRiskType = WinRiskType.Win;
            BetType = BetType.Spread;
            TeamType = Some TeamType.Team2;
            SideType = None
            LineId = 618174426;
            AltLineId = Some 7698623884L;
            CustomerReference = None;
            Pitcher1MustStart = None;
            Pitcher2MustStart = None
        }
    let responseJson = client.PlaceBet request |> Async.RunSynchronously
    let response = Compact.deserialize<PlaceBetResponse> responseJson
    printfn "%A" response
    *)
    0
