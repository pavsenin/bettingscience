open System.Net
open PinnacleClient
open System.Configuration
open System
open PinnacleDomain
open System.Collections.Generic
open Microsoft.FSharpLu.Json

[<EntryPoint>]
let main argv =
    let appSettings = ConfigurationManager.AppSettings
    let proxyUrl, proxyLogin, proxyPassword = appSettings.["ProxyUrl"], appSettings.["ProxyLogin"], appSettings.["ProxyPassword"]
    let login, password = appSettings.["Login"], appSettings.["Password"]
    ServicePointManager.SecurityProtocol <- SecurityProtocolType.Tls12
    let proxy = Some(proxyUrl, proxyLogin, proxyPassword)
    let client = PinnacleClient(login, password, proxy)
    // get balance
    let balance = client.GetBalance() |> Async.RunSynchronously
    printfn "%A" balance

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

    let parseDate value =
        DateTime.Parse value
    let fetchSportData (sport:Sport) =
        let leaguesFolder (eventsMap:Dictionary<int64, PeriodData array>) (state:Dictionary<int, LeagueData>) (item:FixturesLeague) =
            let events = item.Events |> Array.map (fun e ->
                let periods = match eventsMap.TryGetValue e.Id with | true, value -> value | false, _ -> [||]
                { Id = e.Id; Starts = parseDate e.Starts; HomeTeam = e.Home; AwayTeam = e.Away; Periods = periods }
            )
            let league = { Id = item.Id; Name = item.Name; Events = events }
            state.Add(item.Id, league) |> ignore
            state
        let getPeriod (period:OddsPeriod) =
            let getHome (value:OddsTeamTotals) = value.Home
            let getAway (value:OddsTeamTotals) = value.Away
            let getTeamTotal (getValue:OddsTeamTotals->OddsTeamTotal) =
                match period.TeamTotal with
                | None -> { Points = 0.0; Over = 0.0; Under = 0.0 }
                | Some total ->
                    let value = getValue total
                    { Points = value.Points; Over = value.Over; Under = value.Under }
            let teamTotalHome, teamTotalAway = getTeamTotal getHome, getTeamTotal getAway
            let money =
                match period.Moneyline with
                | None -> { Home = 0.0; Away = 0.0; Draw = 0.0 }
                | Some value -> { Home = value.Home; Away = value.Away; Draw = value.Draw }
            {
                LineId = period.LineId
                Number = period.Number
                Cutoff = parseDate period.Cutoff
                Spreads = [||]
                Moneyline = money
                Totals = [||]
                TeamTotals = { Home = teamTotalHome; Away = teamTotalAway }
            }
        async {
            let sportId = sport.Id.ToString()
            let! leagues = client.GetLeagues [("sportId", sportId)]
            let offeredLeagues = leagues.Leagues |> Array.filter (fun l -> l.HasOfferings)
            let leagueIdsValue = String.Join(",", offeredLeagues |> Array.map (fun l -> l.Id))

            let! odds = client.GetOdds [("sportId", sportId); ("leagueIds[]", leagueIdsValue); ("oddsFormat", OddsFormat.DECIMAL.ToString())]
            let eventsMap =
                odds.Leagues
                |> Array.fold (fun (state:Dictionary<int64, PeriodData array>) item ->
                    item.Events |> Array.iter (fun e ->
                        let periods = e.Periods |> Array.map getPeriod
                        state.Add(e.Id, periods) |> ignore
                    )
                    state
                ) (new Dictionary<int64, PeriodData array>())

            let! fixtures = client.GetFixtures [("sportId", sportId); ("leagueIds[]", leagueIdsValue)]
            let leaguesMap = fixtures.League |> Array.fold (leaguesFolder eventsMap) (new Dictionary<int, LeagueData>())
            let leagues = leaguesMap |> Seq.map (fun pair -> pair.Value) |> Array.ofSeq
            return { Id = sport.Id; Name = sport.Name; Leagues = leagues }
        }

    // get sports
    let sports =
        client.GetSports()
        |> Async.RunSynchronously
        |> (fun s -> s.Sports)
        //|> Array.filter (fun s -> s.HasOfferings)
        |> Array.filter (fun s -> s.Id = 24)
        |> Array.map fetchSportData
        |> Async.Parallel
        |> Async.RunSynchronously
    printfn "%A" sports

    let convertPeriod (period:PeriodData) =
        let text = sprintf "%d (%s) |%f|%f|%f|" period.LineId (period.Cutoff.ToString()) period.Moneyline.Home period.Moneyline.Draw period.Moneyline.Away
        { Id = period.LineId.ToString(); Text = text; Children = [||] }
    let convertEvent (event:EventData) =
        let text = sprintf "%s - %s (%s)" event.HomeTeam event.AwayTeam (event.Starts.ToString())
        let children = event.Periods |> Array.map convertPeriod
        { Id = event.Id.ToString(); Text = text; Children = children }
    let convertLeague (league:LeagueData) =
        let children = league.Events |> Array.map convertEvent
        { Id = league.Id.ToString() + league.Name; Text = league.Name; Children = children }
    let convertSport (sport:SportData) =
        let children = sport.Leagues |> Array.map convertLeague
        { Id = sport.Id.ToString() + sport.Name; Text = sport.Name; Children = children }
    let values = sports |> Array.map convertSport
    Compact.serializeToFile "sports.json" values
    0
