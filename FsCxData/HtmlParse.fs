module HtmlParse

open FSharp.Data

[<Literal>] 
let Url = @"http://results.smartiming.co.uk/view-race/ycca1617winterround2senior"
//let url = "..\..\..\CX\Html\Race Times.html"
//let url = @"C:\Users\Adam\Dropbox\Work\Dev\Cyclocross\CX\Html\Race Times.html"

type resultsProvider = HtmlProvider<Url>

type LapColumn = { 
    index: int; 
    lapNumber: int
}  

let toLapColumn index lapNumber = { index = index; lapNumber = lapNumber }

let parseLapNumber (text:string) = 
    match text with
    | text when text.Length <= 3 -> None
    | text when text = "Laps" -> None
    | text when text.Substring(0,3) = "Lap" -> Some(text.Substring(3) |> System.Int32.Parse)
    | _ -> None

let table = resultsProvider.Load(Url).Tables.Smartiming

let lapColumns = 
    table.Headers.Value
    |> Seq.mapi (fun i header -> (i, header |> parseLapNumber))
    |> Seq.filter(fun (_ , header) -> header.IsSome)
    |> Seq.map (fun (i, header) -> toLapColumn i header.Value)

