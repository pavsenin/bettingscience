module Utils
open System
open System.Net
open FSharp.Data
open HtmlAgilityPack

let defArg defaultValue arg = defaultArg arg defaultValue
let (|>>) v f = try v |> Option.map f with | _ -> None
let (||>) v f = try v |> Option.bind f with | _ -> None

let merge2 x1 x2 =
    match x1, x2 with
    | Some v1, Some v2 -> Some (v1, v2)
    | _ -> None
let merge3 x1 x2 x3 =
    match x1, x2, x3 with
    | Some v1, Some v2, Some v3 -> Some (v1, v2, v3)
    | _ -> None

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

let asString (value:JsonValue) = value.AsString()
let asFloat (value:JsonValue) = float32(value.AsFloat())
let getAttribute (node:HtmlNode) func =
    node.Attributes |> List.ofSeq |> List.tryFind func