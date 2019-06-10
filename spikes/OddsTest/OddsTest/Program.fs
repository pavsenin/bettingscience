open System
open System.IO
open System.Net
open FSharp.Data
open FSharp.Data.JsonExtensions
open HtmlAgilityPack

let defArg defaultValue arg = defaultArg arg defaultValue
let (|>>) v f = v |> Option.map f
let (||>) v f = v |> Option.bind f

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
    let json = extractDataFromResponse id content |> Option.map JsonValue.Parse
    json |> Option.map (fun value ->
        let odds = value?d?oddsdata?back.["E-1-2-0-0-0"]?odds
        let outcomeID = value?d?oddsdata?back.["E-1-2-0-0-0"]?OutcomeID
        let history0 = value?d?history?back.[outcomeID.["0"].AsString()]
        let history1 = value?d?history?back.[outcomeID.["1"].AsString()]
        let history2 = value?d?history?back.[outcomeID.["2"].AsString()]

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

let parseXhashKey url =
    let html = fetchContent url matchHost url
    let document = HtmlDocument()
    document.LoadHtml(html)
    let scripts = document.DocumentNode.SelectNodes("/html/body/script") |> List.ofSeq
    let result =
        scripts |> Seq.tryPick (fun script ->
            let text = script.InnerText
            let idStartText = "\"id\":\""
            let endText = "\",\""
            let idStart = text.IndexOf idStartText
            let idEnd = text.IndexOf endText
            if idStart < 0 || idEnd < idStart then None
            else
                let start = idStart + idStartText.Length
                let id = text.Substring(start, idEnd - start)
                let restText = text.Substring(idEnd + endText.Length)
                let hashStartText = "xhash\":\""
                let hashStart = restText.IndexOf hashStartText
                let hashEnd = restText.IndexOf "\",\""
                if hashStart < 0 || hashEnd < hashStart then None
                else
                    let start = hashStart + hashStartText.Length
                    let hash = restText.Substring(start, hashEnd - start)
                    Some(id, hash)
        )
    result

let getAttribute (node:HtmlNode) func =
    node.Attributes |> List.ofSeq |> List.tryFind func

[<EntryPoint>]
let main argv =
    //jytwvQhq RFL1819
    //hdM4QuuS RFL1718
    //dSBJYVTs RFL1617
    let url = "https://fb.oddsportal.com/ajax-sport-country-tournament-archive/1/jytwvQhq/X0/1/0/1/?_=" + fromUnixTimestamp()
    let content = fetchContent url "fb.oddsportal.com" "https://www.oddsportal.com/"
    let json = extractDataFromResponse "/ajax-sport-country-tournament-archive/1/jytwvQhq/X0/1/0/1/" content |> Option.map JsonValue.Parse
    match json with
    | None -> 0
    | Some value ->
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
                match xeid, matchUrl with
                | Some v1, Some v2 -> Some (v1, v2)
                | _ -> None
            )
        let (matchID, matchRelativeUrl) = matches.[5]
        let matchUrl = "http://www.oddsportal.com/" + matchRelativeUrl
        let xhash = parseXhashKey matchUrl

        let matchData = "/feed/match/1-1-" + matchID + "-1-2-yjf94.dat"
        let matchDataUrl = "https://fb.oddsportal.com" + matchData + "?_=" + fromUnixTimestamp()
        let matchContent = fetchContent matchDataUrl "fb.oddsportal.com" "https://www.oddsportal.com/"
        let pinnacleOdds = parseFootballMatchResponse matchData matchContent
        0
    |> ignore
    //https://www.oddsportal.com/soccer/russia/premier-league-2017-2018/results/

    //let url = "https://fb.oddsportal.com/feed/match/1-1-hjTtp2r3-1-2-yjddf.dat?_=" + time
    //let url = "https//fb.oddsportal.com/feed/postmatchscore/1-hjTtp2r3-yjc78.dat?_=" + time
    //let content = fetchContent url "fb.oddsportal.com" "https://www.oddsportal.com/soccer/estonia/esiliiga/levadia-keila-jk-hjTtp2r3/"
    //let json = extractDataFromResponse "/feed/postmatchscore/1-hjTtp2r3-yjc78.dat" content |> Option.map JsonValue.Parse
    //match json with
    //| None -> 0
    //| Some value ->
    //    let odds = value?d?oddsdata?back.["E-1-2-0-0-0"]?odds
    //    let outcomeID = value?d?oddsdata?back.["E-1-2-0-0-0"]?OutcomeID
    //    let history0 = value?d?history?back.[outcomeID.["0"].AsString()]
    //    let history1 = value?d?history?back.[outcomeID.["1"].AsString()]
    //    let history2 = value?d?history?back.[outcomeID.["2"].AsString()]

    //    let pinnacleOdds = odds.[pinnacleID]
    //    let pinnacleHistory0 = history0.[pinnacleID]
    //    let pinnacleHistory1 = history1.[pinnacleID]
    //    let pinnacleHistory2 = history2.[pinnacleID]
    //    0
    //|> ignore

    0 // return an integer exit code
