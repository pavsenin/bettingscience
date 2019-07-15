module OddsPortalScraper
open System
open Utils
open Domain
open FSharp.Data
open FSharp.Data.JsonExtensions
open HtmlAgilityPack
open Microsoft.FSharpLu.Json
open System.Net

let oddsportalHost = "www.oddsportal.com"

let soccerID = "1", "2"
let baseballID = "6", "1"
let basketballID = "3", "1"

let out1x2ID, outOverUnderID, outHomeAwayID, outAsianHandicapID = "1", "2", "3", "5"

let pinnacleID, _1xbetID, asianoddsID, _188betID, bet365ID, betfairID, bwinID, marafonID, winlineID, dafabetID, sbobetID =
    "18", "417", "476", "56", "419", "429", "2", "381", "454", "147", "75"
let bookIDs = [|
    pinnacleID; // sbobetID; dafabetID; _188betID;
    betfairID; // asianoddsID;
    bet365ID; // bwinID;
    marafonID; //_1xbetID; winlineID;
|]

let getBook bookID books = books |> Array.tryFind (fun (key, _) -> key = bookID)

let getOutcomes3 asFunc = function
    | JsonValue.Array [|o1; o0; o2|] ->
        Some(asFunc o1, asFunc o0, asFunc o2)
    | JsonValue.Record _ as value ->
        Some(asFunc value.["0"], asFunc value.["1"], asFunc value.["2"])
    | _ -> None
let getOutcomes2 asFunc = function
    | JsonValue.Array [|o1; o2|] ->
        Some(asFunc o1, asFunc o2)
    | JsonValue.Record _ as value ->
        Some(asFunc value.["0"], asFunc value.["1"])
    | _ -> None
let getStringOutcomes3, getFloatOutcomes3, getIntOutcomes3 =
    getOutcomes3 asString, getOutcomes3 asFloat, getOutcomes3 asInt
let getStringOutcomes2, getFloatOutcomes2, getIntOutcomes2 =
    getOutcomes2 asString, getOutcomes2 asFloat, getOutcomes2 asInt

let getOdds getOutcomes bookID (odds:JsonValue, time:JsonValue) =
    match odds, time with
    | JsonValue.Record booksOdds, JsonValue.Record booksTime ->
        let bookOdds = getBook bookID booksOdds
        let bookTime = getBook bookID booksTime
        match bookOdds, bookTime with
        | Some (_, odds), Some (_, time) ->
            getOutcomes odds time
        | _ -> None
    | _ -> None
let getOdds2 = getOdds (fun odds time ->
        match (getFloatOutcomes2 odds), (getIntOutcomes2 time) with
        | Some (o1, o2), Some (t1, t2) -> Some ((o1, t1), (o2, t2))
        | _ -> None
    )
let getOdds3 = getOdds (fun odds time ->
        match (getFloatOutcomes3 odds), (getIntOutcomes3 time) with
        | Some (o1, o0, o2), Some (t1, t0, t2) -> Some ((o1, t1), (o0, t0), (o2, t2))
        | _ -> None
    )

let getOutcomesOdds getFunc getData (value:JsonValue) =
    let oddsData = value?d?oddsdata?back
    match oddsData with
    | JsonValue.Record data ->
        data |> Array.tryPick (fun (_, value) ->
            let odds, time = getData value
            getFunc (odds, time)
        )
    | _ -> None

let getBookmakersOdds bookIDs getOddsFunc =
    let bookOdds = bookIDs |> Array.choose getOddsFunc
    match bookOdds with
    | [||] -> None
    | _ -> Some [| { Value = None; BookOdds = bookOdds }|]
let getActive bookID act =
    match act with
    | JsonValue.Record booksAct ->
        let bookAct = getBook bookID booksAct
        bookAct |>> (fun (_, act) -> asBool act)
    | _ -> None
    |> defArg false
let getActiveValue (value:JsonValue) =
    let oddsData = value?d?oddsdata?back
    match oddsData with
    | JsonValue.Record data ->
        data |> Array.tryPick (fun (_, value) -> Some value?act)
    | _ -> None
let getHomeAwayOdds bookIDs value =
    let getOdds bookID =
        let starting = getOutcomesOdds (getOdds2 bookID) (fun (value:JsonValue) -> value?opening_odds, value?opening_change_time) value
        let closing = getOutcomesOdds (getOdds2 bookID) (fun (value:JsonValue) -> value?odds, value?change_time) value
        let active = value |> getActiveValue |>> getActive bookID |> defArg false
        if not active then None
        else
            match starting, closing with
            | Some (s1, s2), Some (c1, c2) ->
                Some { BookID = bookID; Odds = { Opening = X2 { O1 = s1; O2 = s2 }; Closing = X2 { O1 = c1; O2 = c2 } } }
            | _ -> None
    getBookmakersOdds bookIDs getOdds
let get1x2Odds bookIDs value =
    let getOdds bookID =
        let starting = getOutcomesOdds (getOdds3 bookID) (fun (value:JsonValue) -> value?opening_odds, value?opening_change_time) value
        let closing = getOutcomesOdds (getOdds3 bookID) (fun (value:JsonValue) -> value?odds, value?change_time) value
        let active = value |> getActiveValue |>> getActive bookID |> defArg false
        if not active then None
        else
            match starting, closing with
            | Some (s1, s0, s2), Some (c1, c0, c2) ->
                Some { BookID = bookID; Odds = { Opening = X3 { O1 = s1; O0 = s0; O2 = s2 }; Closing = X3 { O1 = c1; O0 = c0; O2 = c2 } } }
            | _ -> None
    getBookmakersOdds bookIDs getOdds
let getHandicapOdds bookIDs value =
    let oddsData = value?d?oddsdata?back
    match oddsData with
    | JsonValue.Record data ->
        let values =
            data |> Array.choose (fun (_, innerValue) ->
                let handicap = asFloat innerValue?handicapValue
                let bookOdds =
                    bookIDs |> Array.choose (fun bookID ->
                        let starting = getOdds2 bookID (innerValue?opening_odds, innerValue?opening_change_time)
                        let closing = getOdds2 bookID (innerValue?odds, innerValue?change_time)
                        let active = getActive bookID innerValue?act
                        if not active then None
                        else
                            match starting, closing with
                            | Some (s1, s2), Some (c1, c2) ->
                                Some { BookID = bookID; Odds = { Opening = X2 { O1 = s1; O2 = s2 }; Closing = X2 { O1 = c1; O2 = c2 } } }
                            | _ -> None
                    )
                match bookOdds with | [||] -> None | _ -> Some { Value = Some handicap; BookOdds = bookOdds }
            )
            |> Array.sortBy (fun value -> match value with | { Value = Some handicap } -> handicap | _ -> Single.MinValue)
        values
        |> Some
    | _ -> None

let extractJsonFromResponse filePath (response:string) =
    let start, finish = "globals.jsonpCallback('" + filePath + "', ", ");"
    if response.StartsWith(start) && response.EndsWith(finish) then
        let length = response.Length - start.Length - finish.Length
        let jsonString = response.Substring(start.Length, length)
        let json = JsonValue.Parse jsonString
        Some json
    else
        None
let parseMatchResponse bookIDs outID id content =
    let json = extractJsonFromResponse id content
    json ||> (fun value ->
        if outID = outOverUnderID then getHandicapOdds bookIDs value
        else if outID = outAsianHandicapID then getHandicapOdds bookIDs value
        else if outID = outHomeAwayID then getHomeAwayOdds bookIDs value
        else if outID = out1x2ID then get1x2Odds bookIDs value
        else None
    )
let parseMainMatchPage url =
    let extractXHashKey (text:string) =
        let endText = "\",\""
        let idStart = text.IndexOf "\"id\":\""
        let idEnd = text.IndexOf endText
        if idStart < 0 || idEnd < idStart then None
        else
            let restText = text.Substring(idEnd + endText.Length)
            let hashStartText = "xhash\":\""
            let hashStart = restText.IndexOf hashStartText
            let hashEnd = restText.IndexOf "\",\""
            if hashStart < 0 || hashEnd < hashStart then None
            else
                let start = hashStart + hashStartText.Length
                let xhash = restText.Substring(start, hashEnd - start)
                WebUtility.UrlDecode(xhash) |> Some
    let html = fetchContent url oddsportalHost url
    let document = HtmlDocument()
    document.LoadHtml(html)
    let xhash = document.DocumentNode.SelectNodes("/html/body/script") |> List.ofSeq |> Seq.tryPick (fun script -> extractXHashKey script.InnerText)
    let score =
        let getScore (text:string) =
            match text.Split(':') with
            | [|x1; x2|] -> Some(int(x1), int(x2))
            | _ -> None
        let node = document.DocumentNode.SelectSingleNode("/html/body/div/div/div/div/div/div/div/div/div/div[@xeid]/p/strong")
        if node <> null then
            let scoreText = node.InnerText
            if scoreText.IndexOf("OT") > 0 then
                match scoreText.Split([|"OT"|], StringSplitOptions.RemoveEmptyEntries) with
                | [|x1; x2|] ->
                        let score = getScore (x1.Trim())
                        let scoreWithoutOT = getScore (x2.Trim([|' '; '('; ')'|]))
                        score |>> (fun s -> s, scoreWithoutOT)
                | _ -> None
            else if scoreText.IndexOf("penalties") > 0 then
                let trimmed = scoreText.Substring(0, scoreText.IndexOf("penalties")).Trim()
                let score = getScore trimmed
                score |>> (fun s -> s, None)
            else
                let score = getScore scoreText
                score |>> (fun s -> s, None)
        else
            None
    let periods =
        let node = document.DocumentNode.SelectSingleNode("/html/body/div/div/div/div/div/div/div/div/div/div[@xeid]/p")
        if node <> null then
            let index1, index2 = node.InnerText.LastIndexOf("(") + 1, node.InnerText.LastIndexOf(")")
            if index1 >= 0 && index2 > index1 then
                let data = node.InnerText.Substring(index1, index2 - index1)
                match data.Split(',') with
                | [||] -> None
                | array ->
                    array |> Array.map (fun p ->
                        match p.Split(':') with
                        | [|x1; x2|] -> toInt x1, toInt x2
                        | _ -> -1, -1)
                    |> Some
            else None
        else
            None
    let time =
        let node = document.DocumentNode.SelectSingleNode("/html/body/div/div/div/div/div/div/div/div/div[@id='col-content']/p")
        if node <> null then
            let value = node.GetAttributeValue("class", "")
            let startIndex = "date datet t".Length
            let endIndex = value.IndexOf("-")
            if startIndex >= 0 && endIndex > startIndex then
                let timeValue = value.Substring(startIndex, endIndex - startIndex)
                match Int32.TryParse(timeValue) with
                | true, time -> Some time
                | _ -> None
            else None
        else
            None
    let teams =
        let node = document.DocumentNode.SelectSingleNode("/html/body/div/div/div/div/div/div/div/div/div[@id='col-content']/h1")
        if node <> null then
            match node.InnerText.Split('-') with
            | [|x1; x2|] -> Some(x1.Trim(), x2.Trim())
            | _ -> None
        else
            None
    match xhash, teams, score, periods, time with
    | Some x1, Some x2, Some x3, Some x4, Some x5 -> Some (x1, x2, x3, x4, x5)
    | _ -> None
let extractMatches (document:HtmlDocument) =
    let getMatchUrl node =
        getAttribute node (fun attr -> attr.Name = "class" && attr.Value = "name table-participant") |>> (fun _ ->
            node.ChildNodes |> List.ofSeq |> List.tryFind (fun c -> c.Name = "a") |>> (fun a ->
                getAttribute a (fun attr -> attr.Name = "href") |>> (fun attr -> attr.Value) |> defArg ""
            ) |> defArg ""
        )
    let getMatchData trNode =
        let xeid = getAttribute trNode (fun attr -> attr.Name = "xeid") |>> (fun attr -> attr.Value)
        let matchUrl = trNode.ChildNodes |> List.ofSeq |> List.tryPick getMatchUrl
        match xeid, matchUrl with
        | Some v1, Some v2 -> Some (v1, v2)
        | _ -> None
    document.DocumentNode.SelectNodes("/table/tbody/tr") |> List.ofSeq |> List.choose getMatchData
let extractMatchOdds bookIDs (sportID, dataID) outIDs (matchID, matchRelativeUrl) =
    let getOddsData hash outID =
        let matchData = "/feed/match/1-" + sportID + "-" + matchID + "-" + outID + "-" + dataID + "-" + hash + ".dat"
        let matchDataUrl = "https://fb.oddsportal.com" + matchData + "?_=" + fromUnixTimestamp()
        let matchContent = fetchContent matchDataUrl "fb.oddsportal.com" oddsportalHost
        let odds = parseMatchResponse bookIDs outID matchData matchContent
        odds |>> (fun values -> { OutcomeID = outID; Values = values })
    let getMatchData matchUrl (hash, (teamHome, teamAway), ((score1, score2), scoreWithoutOT), periodsData, time) =
        let odds = outIDs |> List.choose (getOddsData hash) |> List.toArray
        let periods = periodsData |> Array.map (fun (ph, pa) -> { Home = ph; Away = pa })
        let scoreOT = scoreWithoutOT |>> (fun (s1, s2) -> { Home = s1; Away = s2 })
        { ID = matchID; Url = matchUrl; TeamHome = teamHome; TeamAway = teamAway; Time = time;
        Score = { Home = score1; Away = score2 }; ScoreWithoutOT = scoreOT; Periods = periods; Odds = odds }
    let matchUrl = "http://www.oddsportal.com/" + matchRelativeUrl
    let mainData = parseMainMatchPage matchUrl
    mainData |>> getMatchData matchUrl

let extractLeagueMatches (json:JsonValue) =
    let html = json?d?html.AsString()
    let document = HtmlDocument()
    document.LoadHtml(html)
    extractMatches document
let fetchLeagueMatches leagueRelativeUrl pageNum =
    let leagueRelativeUrlNum = leagueRelativeUrl + pageNum.ToString() + "/"
    let url = "https://fb.oddsportal.com" + leagueRelativeUrlNum + "?_=" + fromUnixTimestamp()
    let content = fetchContent url "fb.oddsportal.com" "https://www.oddsportal.com/"
    let json = extractJsonFromResponse leagueRelativeUrlNum content
    json |>> extractLeagueMatches |> defArg []
let fetchLeagueDataAndSaveToFile (sportID, dataID) outIDs (leagueID, pageCount, fileName) =
    let leagueRelativeUrl = "/ajax-sport-country-tournament-archive/" + sportID + "/" + leagueID + "/X0/1/0/"
    let matches =
        [1..pageCount]
        |> List.map (fun pageNum -> fetchLeagueMatches leagueRelativeUrl pageNum)
        |> List.concat
    printfn "%d" matches.Length
    let matchesOdds = matches |> List.mapi (fun i m -> printfn "%d" i; extractMatchOdds bookIDs (sportID, dataID) outIDs m) |> List.choose id |> List.toArray
    let leagueData = { ID = leagueID; Matches = matchesOdds }
    Compact.serializeToFile fileName leagueData

