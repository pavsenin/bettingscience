module Utils
open System
open System.Net
open FSharp.Data
open HtmlAgilityPack

let defArg defaultValue arg = defaultArg arg defaultValue
let (|>>) v f = try v |> Option.map f with | _ -> None
let (||>) v f = try v |> Option.bind f with | _ -> None

let round (r:float32) = float32(Math.Round((decimal)r, 3))
let toInt value =
    match Int32.TryParse value with
    | true, v -> v
    | false, _ -> -1

let fromUnixTimestamp() =
    let origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
    let time = DateTime.UtcNow.Subtract origin
    let ms = time.TotalMilliseconds |> int64
    ms.ToString()

type LongWebClient() =
    inherit WebClient()
    override this.GetWebRequest uri =
        let timeout = 600 * 60 * 1000
        let webRequest = base.GetWebRequest uri
        webRequest.Timeout <- timeout
        match webRequest with | :? HttpWebRequest as httpRequest -> httpRequest.ReadWriteTimeout <- timeout | _ -> () |> ignore
        webRequest

let fetchContent (url:string) host referer =
    let client = new LongWebClient()
    client.Headers.Add("Host", host)
    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36")
    client.Headers.Add("Referer", referer)
    client.Headers.Add("Cookie", "_ga=GA1.2.1789088922.1532722502; _gid=GA1.2.2104236509.1548523833")
    client.DownloadString(url)

let asString (value:JsonValue) = value.AsString()
let asFloat (value:JsonValue) = float32(value.AsFloat())
let asInt (value:JsonValue) = value.AsInteger()
let asBool (value:JsonValue) = value.AsBoolean()

let getAttribute (node:HtmlNode) func =
    node.Attributes |> List.ofSeq |> List.tryFind func