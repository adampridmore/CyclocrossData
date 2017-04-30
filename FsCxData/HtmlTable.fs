module HtmlTable

open FSharp.Data
open FSharp.Charting
open System
open TableTypes

let Load (htmlDocument:HtmlDocument) = 
    let parseColumns (tableNode:HtmlNode) = 
        let thead = tableNode.Descendants("thead") |> Seq.head
        let theardTr = thead.Descendants("tr") |> Seq.head
        theardTr.Descendants("th")
        |> Seq.map(fun (node:HtmlNode) -> node.InnerText())
        |> Seq.toList

    let parseRow (tableRow:HtmlNode) = 
        let values = 
            tableRow.Descendants(["td";"th"]) 
            |> Seq.map (fun value -> value.InnerText())
            |> Seq.toList
            
        {
            values = values |> Seq.toArray
        }
        
    let parseRows (tableNode:HtmlNode) = 
        let tbody = tableNode.Descendants("tbody") |> Seq.head
        tbody.Descendants("tr")
        |> Seq.map parseRow
    
    let htmlToTable (node : HtmlNode) = {
        id = 
            match node.TryGetAttribute("id") with
            | Some(id) -> Some(id.Value())
            | _ -> None
            
        columns = node |> parseColumns |> Seq.toArray;
        rows = node |> parseRows |> Seq.toArray
    }

    htmlDocument.Descendants("table") |> Seq.map htmlToTable

let LoadByUrl (url:String) = 
    HtmlDocument.Load(url) |> Load

let LoadHtml (id:String) (html:System.IO.TextReader) = 
    HtmlDocument.Load(html) |> Load 

let LoadById (id:String) (url:String) = 
    LoadByUrl(url) 
    |> Seq.filter (fun t -> t.id.IsSome)
    |> Seq.filter(fun t -> t.id.Value = id) 
    |> Seq.head


