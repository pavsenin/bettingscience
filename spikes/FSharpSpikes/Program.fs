open System.Net
open PinnacleClient
open System.Configuration
open System
open PinnacleDomain

[<EntryPoint>]
let main argv =
    let appSettings = ConfigurationManager.AppSettings
    let proxyUrl, proxyLogin, proxyPassword = appSettings.["ProxyUrl"], appSettings.["ProxyLogin"], appSettings.["ProxyPassword"]
    let login, password = appSettings.["Login"], appSettings.["Password"]
    ServicePointManager.SecurityProtocol <- SecurityProtocolType.Tls12
    let proxy = Some(proxyUrl, proxyLogin, proxyPassword)
    let client = PinnacleClient(login, password, proxy)
    // get balance
    //let balance = client.GetBalance() |> Async.RunSynchronously
    //printfn "%A" balance

    // get bets
    //let request = [
    //    ("betlist", BetListType.RUNNING.ToString());
    //    ("fromDate", DateTime.Today.ToString("yyyy-MM-dd"));
    //    ("toDate", DateTime.Now.ToString("yyyy-MM-dd"))
    //]
    //let bets = client.GetBets request |> Async.RunSynchronously
    //printfn "%A" bets

    // place bets
    //let request =
    //    {
    //        SportId = 18;
    //        EventId = 931651380L;
    //        Stake = 100.0;
    //        OddsFormat = OddsFormat.DECIMAL;
    //        AcceptBetterLine = true;
    //        UniqueRequestId = Guid.NewGuid();
    //        PeriodNumber = 0;
    //        WinRiskType = WinRiskType.Risk;
    //        BetType = BetType.Spread;
    //        TeamType = Some TeamType.Team2;
    //        SideType = None
    //        LineId = 624801683;
    //        AltLineId = Some 7792101740L;
    //        CustomerReference = None;
    //        Pitcher1MustStart = None;
    //        Pitcher2MustStart = None
    //    }
    //    //{
    //    //    SportId = 41;
    //    //    EventId = 932220553L;
    //    //    Stake = 100.0m;
    //    //    OddsFormat = OddsFormat.DECIMAL;
    //    //    AcceptBetterLine = true;
    //    //    UniqueRequestId = Guid.NewGuid();
    //    //    PeriodNumber = 0;
    //    //    WinRiskType = WinRiskType.Risk;
    //    //    BetType = BetType.MoneyLine;
    //    //    TeamType = Some TeamType.Team2;
    //    //    SideType = None
    //    //    LineId = 624642455;
    //    //    AltLineId = None;
    //    //    CustomerReference = None;
    //    //    Pitcher1MustStart = None;
    //    //    Pitcher2MustStart = None
    //    //}
    //    //{
    //    //    SportId = 29;
    //    //    EventId = 916663454L;
    //    //    Stake = 100.0m;
    //    //    OddsFormat = OddsFormat.DECIMAL;
    //    //    AcceptBetterLine = true;
    //    //    UniqueRequestId = Guid.NewGuid();
    //    //    PeriodNumber = 0;
    //    //    WinRiskType = WinRiskType.Risk;
    //    //    BetType = BetType.Spread;
    //    //    TeamType = Some TeamType.Team2;
    //    //    SideType = None
    //    //    LineId = 618174426;
    //    //    AltLineId = Some 7698623884L;
    //    //    CustomerReference = None;
    //    //    Pitcher1MustStart = None;
    //    //    Pitcher2MustStart = None
    //    //}
    //let response = client.PlaceBet request |> Async.RunSynchronously
    //printfn "%A" response

    // get odds
    let sports = client.GetSports() |> Async.RunSynchronously
    printfn "%A" sports
    0
