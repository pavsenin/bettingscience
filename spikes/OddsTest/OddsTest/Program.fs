open System
open System.IO
open System.Net
open FSharp.Data
open FSharp.Data.JsonExtensions
open HtmlAgilityPack
open System.Security.Policy
open System.Net

let defArg defaultValue arg = defaultArg arg defaultValue
let (|>>) v f = v |> Option.map f
let (||>) v f = v |> Option.bind f
let (|><|) v1 v2 = match v1, v2 with | Some x1, Some x2 -> Some (x1, x2) | _ -> None

let oddsDataHost = "fb.oddsportal.com"
let matchHost = "www.oddsportal.com"
let matchReferer = "http://www.oddsportal.com/soccer/russia/premier-league/lokomotiv-moscow-spartak-moscow-nF3P6xBd/"


let fromUnixTimestamp() =
    let origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
    let time = DateTime.UtcNow.Subtract origin
    let ms = time.TotalMilliseconds |> int64
    ms.ToString()
let fetchContent (url:string) host referer =
    let client = new WebClient()
    client.Headers.Add("Host", host)
    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36")
    client.Headers.Add("Referer", referer)
    client.Headers.Add("Cookie", "_ga=GA1.2.1789088922.1532722502; _gid=GA1.2.2104236509.1548523833")
    client.DownloadString(url)

let extractDataFromResponse dataFilePath (response:string) =
    let start = "globals.jsonpCallback('" + dataFilePath + "', "
    let finish = ");"
    if response.StartsWith(start) && response.EndsWith(finish) then
        let index = start.Length
        let length = response.Length - start.Length - finish.Length
        Some (response.Substring(index, length))
    else
        None

let pinnacleID = "18"

let parseFootballMatchResponse id content =
    let getOutcomeIds (id:JsonValue) =
        match id with
        | JsonValue.Array x ->
            Some("", "", "")
        | JsonValue.Record _ ->
            Some(id.["0"].AsString(), id.["1"].AsString(), id.["2"].AsString())
        | _ -> None
    let json = extractDataFromResponse id content |> Option.map JsonValue.Parse
    json |> Option.map (fun value ->
        let oddsData = value?d?oddsdata?back.["E-1-2-0-0-0"]
        let historyData = value?d?history?back
        let odds, outcomeID = oddsData?odds, oddsData?OutcomeID
        let 
        let (homeId, drawId, awayId) = getOutcomeIds outcomeID
        let history0, history1, history2 = historyData.[homeId], historyData.[drawId], historyData.[awayId]

        let pinnacleOdds = odds.[pinnacleID]
        let pinnacleHistory0 = history0.[pinnacleID]
        let pinnacleHistory1 = history1.[pinnacleID]
        let pinnacleHistory2 = history2.[pinnacleID]

        let homeOddsClosing, drawOddsClosing, awayOddsClosing = pinnacleOdds.["0"].AsFloat(), pinnacleOdds.["1"].AsFloat(), pinnacleOdds.["2"].AsFloat()

        let extractStartingOdds (history:JsonValue) =
            let start = history.AsArray() |> Array.head
            let odds = start.AsArray() |> Array.head
            odds.AsFloat()
        let homeOddsStarting, drawOddsStarting, awayOddsStarting =
            extractStartingOdds pinnacleHistory0,
            extractStartingOdds pinnacleHistory1,
            extractStartingOdds pinnacleHistory2
        
        (homeOddsStarting, drawOddsStarting, awayOddsStarting), (homeOddsClosing, drawOddsClosing, awayOddsClosing)
    )

let parseMainMatchPage url =
    let extractXHashKey (text:string) =
        let idStartText = "\"id\":\""
        let endText = "\",\""
        let idStart = text.IndexOf idStartText
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
    let html = fetchContent url matchHost url
    let document = HtmlDocument()
    document.LoadHtml(html)
    let xhash = document.DocumentNode.SelectNodes("/html/body/script") |> List.ofSeq |> Seq.tryPick (fun script -> extractXHashKey script.InnerText)
    let score =
        document.DocumentNode.SelectSingleNode("/html/body/div/div/div/div/div/div/div/div/div/div[@xeid]/p/strong")
        |> (fun node ->
            if node = null then None
            else
                match node.InnerText.Split(':') with
                | [|x1; x2|] -> Some(int(x1), int(x2))
                | _ -> None
        )
    xhash |><| score

let getAttribute (node:HtmlNode) func =
    node.Attributes |> List.ofSeq |> List.tryFind func

[<EntryPoint>]
let main argv =
    //jytwvQhq RFL1819
    //hdM4QuuS RFL1718
    //dSBJYVTs RFL1617
    let url = "https://fb.oddsportal.com/ajax-sport-country-tournament-archive/1/jytwvQhq/X0/1/0/1/?_=" + fromUnixTimestamp()
    let content = fetchContent url "fb.oddsportal.com" "https://www.oddsportal.com/"
    let json = extractDataFromResponse "/ajax-sport-country-tournament-archive/1/jytwvQhq/X0/1/0/1/" content |>> JsonValue.Parse
    let odds =
        json |>> (fun value ->
            let html = value?d?html.AsString()
            let document = HtmlDocument()
            document.LoadHtml(html)
            let trs = document.DocumentNode.SelectNodes("/table/tbody/tr") |> List.ofSeq
            let matches =
                trs |> List.choose (fun tr ->
                    let xeid = getAttribute tr (fun attr -> attr.Name = "xeid") |>> (fun attr -> attr.Value)
                    let matchUrl = tr.ChildNodes |> List.ofSeq |> List.tryPick (fun child ->
                        getAttribute child (fun attr -> attr.Name = "class" && attr.Value = "name table-participant") |>> (fun _ ->
                            child.ChildNodes |> List.ofSeq |> List.tryFind (fun c -> c.Name = "a") |>> (fun a ->
                                getAttribute a (fun attr -> attr.Name = "href") |>> (fun attr -> attr.Value) |> defArg ""
                            ) |> defArg ""
                        )
                    )
                    xeid |><| matchUrl
                )
            matches |> List.map (fun (matchID, matchRelativeUrl) ->
                let matchUrl = "http://www.oddsportal.com/" + matchRelativeUrl
                let xhash = parseMainMatchPage matchUrl
                xhash |>> (fun (hash, score) ->
                    let matchData = "/feed/match/1-1-" + matchID + "-1-2-" + hash + ".dat"
                    let matchDataUrl = "https://fb.oddsportal.com" + matchData + "?_=" + fromUnixTimestamp()
                    let matchContent = fetchContent matchDataUrl "fb.oddsportal.com" "https://www.oddsportal.com/"
                    let pinnacleOdds = parseFootballMatchResponse matchData matchContent
                    (pinnacleOdds, score)
                )
            )
        )
    0
