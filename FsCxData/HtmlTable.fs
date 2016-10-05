module HtmlTable

open FSharp.Data
open FSharp.Charting
open System

type Row = {
    values : string list
}

type Table = {
    id : string
    columns: string list 
    rows : Row list
}

let Load (htmlDocument:HtmlDocument) = 
    let parseColumns (tableNode:HtmlNode) = 
        let thead = tableNode.Descendants("thead") |> Seq.head
        let theardTr = thead.Descendants("tr") |> Seq.head
        theardTr.Descendants("th") 
        |> Seq.map(fun (node:HtmlNode) -> node.InnerText())
        |> Seq.toList

    let parseRow (tableRow:HtmlNode) = 
        let values = 
            tableRow.Descendants("td") 
            |> Seq.map (fun value -> value.InnerText())
            |> Seq.toList

        {
            values = values
        }
        
    let parseRows (tableNode:HtmlNode) = 
        let tbody = tableNode.Descendants("tbody") |> Seq.head
        tbody.Descendants("tr")
        |> Seq.map parseRow
        |> Seq.toList
    
    let htmlToTable (node : HtmlNode) = {
        id = node.Attribute("id").Value();
        columns = node |> parseColumns;
        rows = node |> parseRows;
    }

    htmlDocument.Descendants("table") |> Seq.map htmlToTable

let LoadByUrl (url:String) = 
    HtmlDocument.Load(url) |> Load

let LoadHtml (id:String) (html:System.IO.TextReader) = 
    HtmlDocument.Load(html) |> Load 

let LoadById (id:String) (url:String) = 
    LoadByUrl(url) 
    |> Seq.filter(fun t -> t.id = id) 
    |> Seq.head


