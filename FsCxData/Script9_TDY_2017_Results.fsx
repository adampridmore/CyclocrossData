#r @"..\packages\FSharp.Data.2.3.2\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"

#load "Types.fs"
#load "TableTypes.fs"

open FSharp.Data
open FSharp.Charting
open System
open TableTypes

let loadHtmlDoc (url:string) = HtmlDocument.Load(url)
let descendantsDoc (name:string) (node:HtmlDocument) = node.Descendants(name)
let descendants (name:string) (node:HtmlNode) = node.Descendants(name)

type rider = {
    bib : string
    finishTime: TimeSpan Option
    name : string
}

let tryParseTimeSpan (text:string) = 
    match TimeSpan.TryParse(text) with
    | true, parsed -> Some(parsed)
    | false, _ -> None

let loadRowsForUrl (url:string) =
    let parseRow (rowNode:HtmlNode) = 
        let row = rowNode |> descendants("td") |> Seq.toArray
        {
            bib = row.[0].InnerText()
            finishTime = row.[1].InnerText() |> tryParseTimeSpan
            name = row.[2].InnerText()
        }

    url 
    |> loadHtmlDoc
    |> descendantsDoc "table" |> Seq.head
    |> descendants "tr" |> Seq.skip 1 // First row is the header (the header is not in a 'thead')
    |> Seq.map parseRow
    
let url = "https://resultsbase.net/event/3765/embed/results?round=7939"

[1..47]
|> Seq.map (fun page -> sprintf "%s&page=%d" url page)
|> Seq.map loadRowsForUrl
|> Seq.collect id
|> Seq.iter (printfn "%A")
