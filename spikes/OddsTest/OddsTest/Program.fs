﻿open System
open System.IO
open System.Net
open FSharp.Data
open FSharp.Data.JsonExtensions
open HtmlAgilityPack
open Domain
open Microsoft.FSharpLu.Json
open Accord.Controls
open Accord.MachineLearning.DecisionTrees
open Accord.Neuro
open Accord.Neuro.Learning
open Accord.Statistics

//let inputs, outputs =
//    matches
//    |> Array.choose (fun m ->
//        let { Starting = starting; Closing = closing } = m.Odds
//        let result = getMatchResult m.Score
//        let realHome = if result = Home then 1 else 0
//        let startingHomeProb = getProbabilities starting
//        let closingHomeProb = getProbabilities closing
//        let homeProbDiff = (closingHomeProb - startingHomeProb) / startingHomeProb
//        //printfn "%f %f %d" startingHomeProb homeProbDiff realHome
//        if startingHomeProb < 0.1f then None
//        else Some([|double(closingHomeProb); double(homeProbDiff)|], realHome)
//    )
//    |> Array.unzip
//ScatterplotBox.Show("MLB", inputs, outputs).SetSymbolSize(1.f).Hold();
//let values =
//    matches
//    |> Array.choose (fun m ->
//        let { Starting = starting; Closing = closing } = m.Odds
//        let result = getMatchResult m.Score
//        let realHome = if result = Home then 1.f else 0.f
//        let startingHomeProb = getProbabilities starting
//        let closingHomeProb = getProbabilities closing
//        let homeProbDiff = (closingHomeProb - startingHomeProb) / startingHomeProb
//        //printfn "%f %f %d" startingHomeProb homeProbDiff realHome
//        if startingHomeProb < 0.1f then None
//        else Some(closingHomeProb, realHome)
//    )
//    |> Array.groupBy (fun (prob, _) -> prob)
//    |> Array.sortBy (fun (prob, _) -> prob)
//    |> Array.filter (fun (_, arr) -> (Array.length arr) >= 15)
//    |> Array.map (fun (prob, arr) -> (prob, arr |> Array.map (fun (_, r) -> r) |> Array.average))
//    |> Array.map (fun (prob, real) -> [|double(prob); double(real)|])
//ScatterplotBox.Show("MLB", values).SetSymbolSize(1.f).Hold();

//let getData fileNames = 
//    fileNames
    //|> List.map (fun fileName ->
    //    let leagueData = Compact.deserializeFile<LeagueData> fileName
    //    leagueData.Matches
    //)
    //|> Array.concat
    //|> Array.choose (fun m ->
    //    let { Starting = starting; Closing = closing } = m.Odds
    //    let result = getMatchResult m.Score
    //    let realHome = if result = Home then 1. else 0.
    //    let startingHomeProb = getProbabilities starting
    //    let closingHomeProb = getProbabilities closing
    //    let homeProbDiff = (closingHomeProb - startingHomeProb) / startingHomeProb
    //    //printfn "%f %f %d" startingHomeProb homeProbDiff realHome
    //    if startingHomeProb < 0.1f then None
    //    else Some([|float(closingHomeProb); float(homeProbDiff)|], [|realHome|])
    //)
    //|> Array.unzip
//let trainInputs, trainOutputs = getData ["MLB18.json";"MLB17.json";"MLB16.json";"MLB15.json";"MLB14.json"]
//let testInputs, testOutputs = getData ["MLB19.json"]
//let teacher = new RandomForestLearning(NumberOfTrees = 100, SampleRatio = 1.0)
//let forest = teacher.Learn(trainInputs, trainOutputs)
//let predicted = forest.Decide(testInputs)
//let mutable correct = 0.f
//[0..testOutputs.Length-1]
//|> List.iter (fun i ->
//    printfn "%d %d" predicted.[i] testOutputs.[i]
//    if predicted.[i] = testOutputs.[i] then correct <- correct + 1.f
//)
//printfn "Correct %f" (correct / float32(testOutputs.Length))

//let func = new SigmoidFunction()
//let network = new ActivationNetwork(func, inputsCount = 2, neuronsCount = [|5; 1|])
//let teacher = new LevenbergMarquardtLearning(network, UseRegularization = true)
//let mutable error = Double.PositiveInfinity
//let mutable previous = 0.0
//[0..29] |> List.iter (fun i ->
//    previous <- error
//    error <- teacher.RunEpoch(trainInputs, trainOutputs)
//    printfn "%f" error
//)
//[0..testInputs.Length-1] |> List.iter (fun i ->
//    let predicted = network.Compute(testInputs.[i])
//    printfn "%f %f %f" testInputs.[i].[0] predicted.[0] testOutputs.[i].[0]
//)

let defArg defaultValue arg = defaultArg arg defaultValue
let (|>>) v f = try v |> Option.map f with | _ -> None
let (||>) v f = try v |> Option.bind f with | _ -> None
let (|><|) v1 v2 = match v1, v2 with | Some x1, Some x2 -> Some (x1, x2) | _ -> None

let oddsportalHost = "www.oddsportal.com"

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
    let start, finish = "globals.jsonpCallback('" + dataFilePath + "', ", ");"
    if response.StartsWith(start) && response.EndsWith(finish) then
        let length = response.Length - start.Length - finish.Length
        Some (response.Substring(start.Length, length))
    else
        None

let soccerID = "1"
let baseballID = "6"
let esportsID = "36"

let out1x2ID = "1"
let outOverUnderID = "2"
let outHomeAwayID = "3"
let outAsianHandicapID = "5"

let pinnacleID = "18"

let asString (value:JsonValue) = value.AsString()
let asFloat (value:JsonValue) = float32(value.AsFloat())
let getAttribute (node:HtmlNode) func =
    node.Attributes |> List.ofSeq |> List.tryFind func

let getOutcomes3 asFunc = function
    | JsonValue.Array [|home; draw; away|] ->
        Some(asFunc home, asFunc draw, asFunc away)
    | JsonValue.Record _ as value ->
        Some(asFunc value.["0"], asFunc value.["1"], asFunc value.["2"])
    | _ -> None
let getOutcomes2 asFunc = function
    | JsonValue.Array [|home; away|] ->
        Some(asFunc home, asFunc away)
    | JsonValue.Record _ as value ->
        Some(asFunc value.["0"], asFunc value.["1"])
    | _ -> None
let getStringOutcomes3, getFloatOutcomes3 = getOutcomes3 asString, getOutcomes3 asFloat
let getStringOutcomes2, getFloatOutcomes2 = getOutcomes2 asString, getOutcomes2 asFloat

let getStartingClosingOdds3 (historyData:JsonValue) pinnacleOdds outcomeID =
    getStringOutcomes3 outcomeID ||> (fun (homeId, drawId, awayId) ->
        let pinnacleHistory0, pinnacleHistory1, pinnacleHistory2 =
            historyData.[homeId].[pinnacleID], historyData.[drawId].[pinnacleID], historyData.[awayId].[pinnacleID]
            
        getFloatOutcomes3 pinnacleOdds |>> (fun (homeOddsClosing, drawOddsClosing, awayOddsClosing) ->
            let extractStartingOdds (history:JsonValue) =
                let start = history.AsArray() |> Array.head
                let odds = start.AsArray() |> Array.head
                asFloat odds
            let homeOddsStarting, drawOddsStarting, awayOddsStarting =
                extractStartingOdds pinnacleHistory0, extractStartingOdds pinnacleHistory1, extractStartingOdds pinnacleHistory2
            {
                Starting = X3 { Home = homeOddsStarting; Draw = drawOddsStarting; Away = awayOddsStarting };
                Closing = X3 { Home = homeOddsClosing; Draw = drawOddsClosing; Away = awayOddsClosing }
            }
        )
    )
let getStartingClosingOdds2 (historyData:JsonValue) pinnacleOdds outcomeID =
    getStringOutcomes2 outcomeID ||> (fun (homeId, awayId) ->
        let pinnacleHistory0, pinnacleHistory1 =
            historyData.[homeId].[pinnacleID], historyData.[awayId].[pinnacleID]
            
        getFloatOutcomes2 pinnacleOdds |>> (fun (homeOddsClosing, awayOddsClosing) ->
            let extractStartingOdds (history:JsonValue) =
                let start = history.AsArray() |> Array.head
                let odds = start.AsArray() |> Array.head
                asFloat odds
            let homeOddsStarting, awayOddsStarting =
                extractStartingOdds pinnacleHistory0, extractStartingOdds pinnacleHistory1
            {
                Starting = X2 { Home = homeOddsStarting; Away = awayOddsStarting };
                Closing = X2 { Home = homeOddsClosing; Away = awayOddsClosing }
            }
        )
    )

let parseFootballMatchResponse (outID, hzID) id content =
    let json = extractDataFromResponse id content |> Option.map JsonValue.Parse
    json ||> (fun value ->
        let xxx =
            match value?d?oddsdata?back with
            | JsonValue.Record data ->
                data
                |> Array.choose (fun (_, value) ->
                    let handicap = value?handicapValue
                    let pinnacleOdds = 
                        match value?odds with
                        | JsonValue.Record books ->
                            books |> Array.tryFind (fun (key, _) -> key = pinnacleID)
                            ||> (fun (_, value) ->
                                match value with
                                | JsonValue.Array [|o; u|] -> Some (o, u)
                                | _ -> None
                            )
                        | _ -> None
                    pinnacleOdds |>> (fun odds -> (handicap, odds))
                ) |> Array.sortBy (fun (key, _) -> key)
                |> Some
            | _ -> None
        let oddsData = value?d?oddsdata?back.["E-" + outID + "-" + hzID + "-0-0-0"]
        let historyData, pinnacleOdds, outcomeID = value?d?history?back, oddsData?odds.[pinnacleID], oddsData?OutcomeID
        match outID with
        | "1" -> getStartingClosingOdds3 historyData pinnacleOdds outcomeID
        | "3" -> getStartingClosingOdds2 historyData pinnacleOdds outcomeID
        | _ -> None
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
    let html = fetchContent url oddsportalHost url
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

let extractMatches (document:HtmlDocument) =
    document.DocumentNode.SelectNodes("/table/tbody/tr") |> List.ofSeq |> List.choose (fun tr ->
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

let extractOdds (sportID, outID, hzID) (matchID, matchRelativeUrl) =
    let matchUrl = "http://www.oddsportal.com/" + matchRelativeUrl
    let xhash = parseMainMatchPage matchUrl
    xhash ||> (fun (hash, (score1, score2)) ->
        let matchData = "/feed/match/1-" + sportID + "-" + matchID + "-" + outID + "-" + hzID + "-" + hash + ".dat"
        let matchDataUrl = "https://fb.oddsportal.com" + matchData + "?_=" + fromUnixTimestamp()
        let matchContent = fetchContent matchDataUrl "fb.oddsportal.com" "https://www.oddsportal.com/"
        let pinnacleOdds = parseFootballMatchResponse (outID, hzID) matchData matchContent
        pinnacleOdds |>> (fun odds -> { ID = matchID; Url = matchUrl; Score = { Home = score1; Away = score2 }; Odds = odds })
    )

type MatchResult = Home | Draw | Away
let getMatchResult ({ Home = home; Away = away }:MatchScore) =
    if home > away then Home else if home = away then Draw else Away

let getProbabilities = function
    | X3 { Home = home; Draw = draw; Away = away } ->
        let homeProb, drawProb, awayProb = 1.f / home, 1.f / draw, 1.f / away
        let sumProb = homeProb + drawProb + awayProb
        homeProb / sumProb
    | X2 { Home = home; Away = away } ->
        let homeProb, awayProb = 1.f / home, 1.f / away
        let sumProb = homeProb + awayProb
        homeProb / sumProb
let checkEffectiveMarketHypothese fileNames =
    let matches =
        fileNames
        |> List.map (fun fileName ->
            let leagueData = Compact.deserializeFile<LeagueData> fileName
            leagueData.Matches
        )
        |> Array.concat
    printfn "Count %d" matches.Length

    let mutable startingHomeSum = 0.f
    let mutable closingHomeSum = 0.f
    let mutable realHomeSum = 0.f
    for m in matches do
        //printfn "%A" m
        let { Starting = starting; Closing = closing } = m.Odds
        let result = getMatchResult m.Score
        let realHome = if result = Home then 1.f else 0.f
        let startingHomeProb = getProbabilities starting
        let closingHomeProb = getProbabilities closing
        realHomeSum <- realHomeSum + realHome
        startingHomeSum <- startingHomeSum + startingHomeProb
        closingHomeSum <- closingHomeSum + closingHomeProb
        //printfn "Starting %f, Closing %f, Real %f" startingHomeProb closingHomeProb realHome
        printfn "StartingSum %f, ClosingSum %f" (realHomeSum - startingHomeSum) (realHomeSum - closingHomeSum)
        printfn ""

// ("jytwvQhq", 5, "RFL1819.json"), ("hdM4QuuS", 5, "RFL1718.json"), ("dSBJYVTs", 5, "RFL1617.json"),
// ("GQkWIAQ7", 5, "RFL1516.json"), ("Kh7n2gWp", 5, "RFL1415.json"), ("ITC1yoVJ", 5, "RFL1314.json")
// ("fTANbXxl", 6, "OWL18.json")
// ("xdttaT5s", 3, "LoLC19.json"), ("UqMHACKl", 8, "LoLC18.json"), ("tQQ0iGoN", 7, "LoLC17.json"), ("Ys6Qr462", 8, "LoLC16.json")
// ("W2c1dRk0", 4, "LoLChina19.json"), ("OIF6g7Tp", 6, "LoLChina18.json"), ("hOlI07Jp", 5, "LoLChina17.json")
// ("Uanezsbs", 30, "MLB19.json"), ("r3414Mwe", 58, "MLB18.json"), ("bwFloypH", 58, "MLB17.json"), ("67blnzDc", 57, "MLB16.json"), ("QgQMkPOM", 57, "MLB15.json"), ("Y9I8VpDI", 57, "MLB14.json")


let fetchLeagueDataAndSaveToFile sportID outID (leagueID, pageCount, fileName) =
    let leagueRelativeUrl = "/ajax-sport-country-tournament-archive/" + sportID + "/" + leagueID + "/X0/1/0/"
    let matches =
        [1..pageCount]
        |> List.map (fun pageNum ->
            let leagueRelativeUrlNum = leagueRelativeUrl + pageNum.ToString() + "/"
            let url = "https://fb.oddsportal.com" + leagueRelativeUrlNum + "?_=" + fromUnixTimestamp()
            let content = fetchContent url "fb.oddsportal.com" "https://www.oddsportal.com/"
            let json = extractDataFromResponse leagueRelativeUrlNum content |>> JsonValue.Parse
            json |>> (fun value ->
                let html = value?d?html.AsString()
                let document = HtmlDocument()
                document.LoadHtml(html)
                extractMatches document |> List.map (extractOdds (sportID, outID, "1"))
            ) |> defArg []
        )
        |> List.concat |> List.choose id |> List.toArray
    let leagueData = { ID = leagueID; Matches = matches }
    Compact.serializeToFile fileName leagueData

[<EntryPoint>]
let main argv =
    fetchLeagueDataAndSaveToFile baseballID outOverUnderID ("Y9I8VpDI", 57, "MLB14.json")
    //checkEffectiveMarketHypothese ["MLB19.json";"MLB18.json";"MLB17.json";"MLB16.json";"MLB15.json";"MLB14.json"]

    0
