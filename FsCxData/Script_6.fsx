#r @"..\packages\FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"
#r @"System.Windows.Forms.DataVisualization.dll"

#load "HtmlTable.fs"

open FSharp.Data
open FSharp.Charting
open System

let url = @"http://results.smartiming.co.uk/view-race/ycca1617winterround4senior/" // RD4
let table = url |> HtmlTable.LoadById "dt-user-list"

table.columns.[0]

//    
//let doc = FSharp.Data.HtmlDocument.Load(Url)
//
//type Row = {
//    values : string list
//}
//
//type Table = {
//    id : string
//    columns: string seq 
//    rows : Row list
//}
//
//let parseColumns (tableNode:HtmlNode) = 
//    let thead = tableNode.Descendants("thead") |> Seq.head
//    let theardTr = thead.Descendants("tr") |> Seq.head
//    theardTr.Descendants("th") |> Seq.map(fun (node:HtmlNode) -> node.InnerText());
//
//let parseRow (tableRow:HtmlNode) = 
//    let values = 
//        tableRow.Descendants("td") 
//        |> Seq.map (fun value -> value.InnerText())
//        |> Seq.toList
//
//    {
//        values = values
//    }
//        
//let parseRows (tableNode:HtmlNode) = 
//    let tbody = tableNode.Descendants("tbody") |> Seq.head
//    tbody.Descendants("tr")
//    |> Seq.map parseRow
//    |> Seq.toList
//    
//let htmlToTable (node : HtmlNode) = {
//    id = node.Attribute("id").Value();
//    columns = node |> parseColumns;
//    rows = node |> parseRows;
//}
//
//let table = doc.Descendants("table") |> Seq.head;
//let thead = (table.Descendants("thead")) |> Seq.head
//let theardTr = thead.Descendants("tr") |> Seq.head
//theardTr.Descendants("th") |> Seq.map(fun (node:HtmlNode) -> node.InnerText());
//
//doc.Descendants("table") 
//|> Seq.map(fun node -> node |> htmlToTable)
