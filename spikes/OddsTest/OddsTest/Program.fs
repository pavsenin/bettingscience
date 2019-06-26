open System
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
open Accord.Neuro.Networks
open Accord.Neuro.ActivationFunctions

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
let getStringOutcomes3, getFloatOutcomes3 = getOutcomes3 asString, getOutcomes3 asFloat
let getStringOutcomes2, getFloatOutcomes2 = getOutcomes2 asString, getOutcomes2 asFloat

let extractStartingOdds (history:JsonValue) =
    let start = history.AsArray() |> Array.head
    let odds = start.AsArray() |> Array.head
    asFloat odds

//let getStartingClosingOdds3 (historyData:JsonValue) pinnacleOdds outcomeID =
//    getStringOutcomes3 outcomeID ||> (fun (homeId, drawId, awayId) ->
//        let pinnacleHistory0, pinnacleHistory1, pinnacleHistory2 =
//            historyData.[homeId].[pinnacleID], historyData.[drawId].[pinnacleID], historyData.[awayId].[pinnacleID]
//            
//        getFloatOutcomes3 pinnacleOdds |>> (fun (homeOddsClosing, drawOddsClosing, awayOddsClosing) ->
//            let homeOddsStarting, drawOddsStarting, awayOddsStarting =
//                extractStartingOdds pinnacleHistory0, extractStartingOdds pinnacleHistory1, extractStartingOdds pinnacleHistory2
//            {
//                Starting = X3 { Home = homeOddsStarting; Draw = drawOddsStarting; Away = awayOddsStarting };
//                Closing = X3 { Home = homeOddsClosing; Draw = drawOddsClosing; Away = awayOddsClosing }
//            }
//        )
//    )
let getStartingOdds2 (value:JsonValue) =
    let oddsData = value?d?oddsdata?back
    let history = value?d?history?back
    match oddsData with
    | JsonValue.Record data ->
        data |> Array.tryPick (fun (_, value) ->
            getStringOutcomes2 value?OutcomeID |>> (fun (o1, o2) ->
                extractStartingOdds history.[o1].[pinnacleID], extractStartingOdds history.[o2].[pinnacleID]
            )
        )
    | _ -> None
let getClosingOdds2 (value:JsonValue) =
    let oddsData = value?d?oddsdata?back
    match oddsData with
    | JsonValue.Record data ->
        data |> Array.tryPick (fun (_, value) ->
            getFloatOutcomes2 value?odds.[pinnacleID]
        )
    | _ -> None
let getStartingClosingOdds2 (value:JsonValue) =
    let starting = getStartingOdds2 value
    let closing = getClosingOdds2 value
    match starting, closing with
    | Some (s1, s2), Some (c1, c2) ->
        Some [|{
            Value = None;
            Odds = {
                Starting = X2 { O1 = s1; O2 = s2 };
                Closing = X2 { O1 = c1; O2 = c2 }
            }
        }|]
    | _ -> None

let getOverUnderOdds value =
    let history = value?d?history?back
    let oddsData = value?d?oddsdata?back
    match oddsData with
    | JsonValue.Record data ->
        data |> Array.choose (fun (_, innerValue) ->
            let handicap = innerValue?handicapValue
            let starting =
                getStringOutcomes2 innerValue?OutcomeID |>> (fun (o1, o2) ->
                    extractStartingOdds history.[o1].[pinnacleID], extractStartingOdds history.[o2].[pinnacleID]
            )
            let closing = 
                match innerValue?odds with
                | JsonValue.Record books ->
                    books |> Array.tryFind (fun (key, _) -> key = pinnacleID) ||> (fun (_, value) -> getFloatOutcomes2 value)
                | _ -> None
            match starting, closing with
            | Some (so1, so2), Some (co1, co2) ->
                Some (asFloat handicap, (so1, so2), (co1, co2))
            | _ -> None
        )
        |> Array.sortBy (fun (handicap, _, _) -> handicap)
        |> Array.map (fun (handicap, (s1, s2), (c1, c2)) -> {
                Value = Some handicap;
                Odds = {
                    Starting = X2 { O1 = s1; O2 = s2 };
                    Closing = X2 { O1 = c1; O2 = c2 }
                }
            }
        )
        |> Some
    | _ -> None
let parseFootballMatchResponseNew outID id content =
    let json = extractDataFromResponse id content |> Option.map JsonValue.Parse
    json ||> (fun value ->
        if outID = outOverUnderID then getOverUnderOdds value
        else if outID = outHomeAwayID then getStartingClosingOdds2 value
        //else if outID = out1x2ID then
        //    let oddsData = value?d?oddsdata?back.["E-" + outID + "-" + hzID + "-0-0-0"]
        //    let historyData, pinnacleOdds, outcomeID = value?d?history?back, oddsData?odds.[pinnacleID], oddsData?OutcomeID
        //    getStartingClosingOdds3 historyData pinnacleOdds outcomeID
        else None
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
        //| "1" -> getStartingClosingOdds3 historyData pinnacleOdds outcomeID
        //| "3" -> getStartingClosingOdds2 historyData pinnacleOdds outcomeID
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
    let time =
        document.DocumentNode.SelectSingleNode("/html/body/div/div/div/div/div/div/div/div/div[@id='col-content']/p")
        |> (fun node ->
            if node = null then None
            else
                match node.GetAttributeValue("class", "") with
                | value ->
                    let startIndex = "date datet t".Length
                    let endIndex = value.IndexOf("-")
                    if startIndex >= 0 && endIndex > startIndex then
                        let timeValue = value.Substring(startIndex, endIndex - startIndex)
                        match Int32.TryParse(timeValue) with
                        | true, time -> Some time
                        | _ -> None
                    else None
                | _ -> None
        )
    match xhash, score, time with
    | Some x1, Some x2, Some x3 -> Some (x1, x2, x3)
    | _ -> None

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

let extractOdds (sportID, outIDs, hzID) (matchID, matchRelativeUrl) =
    let matchUrl = "http://www.oddsportal.com/" + matchRelativeUrl
    let xhash = parseMainMatchPage matchUrl
    xhash |>> (fun (hash, (score1, score2), time) ->
        let odds =
            outIDs |> List.choose (fun outID ->
                let matchData = "/feed/match/1-" + sportID + "-" + matchID + "-" + outID + "-" + hzID + "-" + hash + ".dat"
                let matchDataUrl = "https://fb.oddsportal.com" + matchData + "?_=" + fromUnixTimestamp()
                let matchContent = fetchContent matchDataUrl "fb.oddsportal.com" "https://www.oddsportal.com/"
                let odds = parseFootballMatchResponseNew outID matchData matchContent
                odds |>> (fun values -> { OutcomeID = outID; Values = values })
            ) |> List.toArray
        { ID = matchID; Url = matchUrl; Time = time; Score = { Home = score1; Away = score2 }; Odds = odds }
    )

type MatchResult = Home | Draw | Away
let getMatchResult ({ Home = home; Away = away }:MatchScore) =
    if home > away then Home else if home = away then Draw else Away

let getProbabilities = function
    | X3 { O1 = o1; O0 = o0; O2 = o2 } ->
        let o1Prob, o0Prob, o2Prob = 1.f / o1, 1.f / o0, 1.f / o2
        let sumProb = o1Prob + o0Prob + o2Prob
        o1Prob / sumProb
    | X2 { O1 = o1; O2 = o2 } ->
        let o1Prob, o2Prob = 1.f / o1, 1.f / o2
        let sumProb = o1Prob + o2Prob
        o1Prob / sumProb
let checkEffectiveMarketHypothese fileNames =
    let matches =
        fileNames
        |> List.map (fun fileName ->
            let leagueData = Compact.deserializeFile<LeagueData> fileName
            leagueData.Matches
        )
        |> Array.concat
    printfn "Count %d" matches.Length

    let mutable startingO1Sum = 0.f
    let mutable closingO1Sum = 0.f
    for m in matches do
        //printfn "%A" m
        let odds = m.Odds |> Array.tryFind (fun out -> out.OutcomeID = outOverUnderID)
        odds |>> (fun odd ->
            let min =
                odd.Values
                |> Array.rev
                |> Array.filter (fun value -> value.Value.IsSome)
                |> Array.head
            match min with
            | { Value = Some value; Odds = { Starting = X2 { O1 = s1; O2 = s2 }; Closing = X2 { O1 = c1; O2 = c2 } } } ->
                printfn "Score (%A:%A) Value %A Starting %A Closing %A" m.Score.Home m.Score.Away value s2 c2
                let result = m.Score.Home + m.Score.Away |> float32
                let getMoney o =
                    if result < value then o - 1.f
                    else if result = value then 0.f
                    else -1.f
                //let startingO1Prob = getProbabilities starting
                //let closingO1Prob = getProbabilities closing
                startingO1Sum <- startingO1Sum + (getMoney s2)
                closingO1Sum <- closingO1Sum + (getMoney c2)
                //printfn "Starting %f, Closing %f, Real %f" startingO1Prob closingO1Prob realO1
                printfn "StartingSum %f, ClosingSum %f" startingO1Sum closingO1Sum
                printfn ""
            | _ -> ()
        ) |> ignore

// ("jytwvQhq", 5, "RFL1819.json"), ("hdM4QuuS", 5, "RFL1718.json"), ("dSBJYVTs", 5, "RFL1617.json"),
// ("GQkWIAQ7", 5, "RFL1516.json"), ("Kh7n2gWp", 5, "RFL1415.json"), ("ITC1yoVJ", 5, "RFL1314.json")
// ("fTANbXxl", 6, "OWL18.json")
// ("xdttaT5s", 3, "LoLC19.json"), ("UqMHACKl", 8, "LoLC18.json"), ("tQQ0iGoN", 7, "LoLC17.json"), ("Ys6Qr462", 8, "LoLC16.json")
// ("W2c1dRk0", 4, "LoLChina19.json"), ("OIF6g7Tp", 6, "LoLChina18.json"), ("hOlI07Jp", 5, "LoLChina17.json")
// ("Uanezsbs", 30, "MLB19.json"), ("r3414Mwe", 58, "MLB18.json"), ("bwFloypH", 58, "MLB17.json"), ("67blnzDc", 57, "MLB16.json"), ("QgQMkPOM", 57, "MLB15.json"), ("Y9I8VpDI", 57, "MLB14.json")


let fetchLeagueDataAndSaveToFile sportID outIDs (leagueID, pageCount, fileName) =
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
                let matches = extractMatches document
                let odds = matches |> List.map (extractOdds (sportID, outIDs, "1"))
                odds
            ) |> defArg []
        )
        |> List.concat |> List.choose id |> List.toArray
    let leagueData = { ID = leagueID; Matches = matches }
    Compact.serializeToFile fileName leagueData

[<EntryPoint>]
let main argv =
    fetchLeagueDataAndSaveToFile baseballID [outHomeAwayID; outAsianHandicapID; outOverUnderID] ("r3414Mwe", 58, "MLB18.json")
    //checkEffectiveMarketHypothese ["MLB18.json";"MLB17.json"]
    //let network = new DeepBeliefNetwork(new BernoulliFunction(), 1024, 50, 10);
    //let weights = new GaussianWeights(network)
    //weights.Randomize()
    //network.UpdateVisibleWeights()

    //let teacher = new BackPropagationLearning(network, LearningRate = 0.1, Momentum = 0.9)
    //double[][] inputs, outputs;
    //Main.Database.Training.GetInstances(out inputs, out outputs);
    //[0..49]
    //|> List.iter (fun i ->
    //    let error = teacher.RunEpoch(inputs, outputs)
    //    printfn "%f" error
    //)


    //let inputs, outputs =
    //    [|"MLBF18.json";"MLBF17.json"|]
    //    |> Array.map (fun fileName ->
    //        let leagueData = Compact.deserializeFile<LeagueData> fileName
    //        leagueData.Matches
    //    )
    //    |> Array.concat
    //    |> Array.choose (fun m ->
    //        m.Odds
    //        |> Array.tryFind (fun o -> o.OutcomeID = outHomeAwayID)
    //        ||> (fun odds ->
    //            match odds.Values with
    //            | [| { Value = None; Odds = { Starting = starting; Closing = closing } }|] ->
    //                let result = getMatchResult m.Score
    //                let realHome = if result = Home then 1 else 0
    //                let startingHomeProb = getProbabilities starting
    //                let closingHomeProb = getProbabilities closing
    //                let homeProbDiff = (closingHomeProb - startingHomeProb) / startingHomeProb
    //                //printfn "%f %f %d" startingHomeProb homeProbDiff realHome
    //                if startingHomeProb < 0.1f then None
    //                else Some([|double(closingHomeProb); double(homeProbDiff)|], realHome)
    //            | _ -> None
    //        )
    //    ) |> Array.unzip
    //    //|> Array.groupBy (fun (prob, _) -> prob)
    //    //|> Array.sortBy (fun (prob, _) -> prob)
    //    //|> Array.filter (fun (_, arr) -> (Array.length arr) >= 5)
    //    //|> Array.map (fun (prob, arr) -> (prob, arr |> Array.map (fun (_, r) -> r) |> Array.average))
    //    //|> Array.map (fun (prob, real) -> [|double(prob); double(real)|])
    //ScatterplotBox.Show("MLB", inputs, outputs).SetSymbolSize(2.f).Hold();


    let matches =
        [|"MLBF18.json";"MLBF17.json"|]
        |> Array.map (fun fileName ->
            let leagueData = Compact.deserializeFile<LeagueData> fileName
            leagueData.Matches
        ) |> Array.concat

    let mutable startingO1Sum = 0.f
    let mutable closingO1Sum = 0.f
    let mutable count = 0
    for m in matches do
        //printfn "%A" m
        m.Odds
        |> Array.tryFind (fun out -> out.OutcomeID = outHomeAwayID)
        |>> (fun odd ->
            match odd.Values with
            | [|{ Value = None;
                  Odds = { Starting = X2 { O1 = s1; O2 = s2 } as starting;
                           Closing = X2 { O1 = c1; O2 = c2 } as closing } }|] ->
                let startingO1Prob = getProbabilities starting
                let closingO1Prob = getProbabilities closing
                if closingO1Prob >= 0.35f then ()
                else if closingO1Prob > 0.3f then
                    printfn "Score (%A:%A) Starting %A Closing %A" m.Score.Home m.Score.Away s2 c2
                    let result = getMatchResult m.Score
                    let getMoney r o = if result = r then o - 1.f else -1.f
                    startingO1Sum <- startingO1Sum + (getMoney Home s1)
                    closingO1Sum <- closingO1Sum + (getMoney Home c1)
                    count <- count + 1
                    printfn "StartingSum %f, ClosingSum %f" startingO1Sum closingO1Sum
                else
                    printfn "Score (%A:%A) Starting %A Closing %A" m.Score.Home m.Score.Away s2 c2
                    let result = getMatchResult m.Score
                    let getMoney r o = if result = r then o - 1.f else -1.f
                    startingO1Sum <- startingO1Sum + (getMoney Away s2)
                    closingO1Sum <- closingO1Sum + (getMoney Away c2)
                    count <- count + 1
                    printfn "StartingSum %f, ClosingSum %f" startingO1Sum closingO1Sum
            | _ -> ()
        ) |> ignore
    printfn "All %d Bet %d" matches.Length count
    0
