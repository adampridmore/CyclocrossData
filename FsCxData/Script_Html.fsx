#r @"..\packages\FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"

#load "Library1.fs"
#load "HtmlParse.fs"

open FsCxData
open FSharp.Data
open FSharp.Charting
open System
open HtmlParse

lapColumns


type Rider = {
    name: string
    laps: TimeSpan list
    cumulativeLapTime : TimeSpan list
    lapCount : int
}

let parseTimeSpan (text:string) =
    try
        ("00:" + text) |> TimeSpan.Parse
    with | _ -> failwithf "Error parsing %s" text

let lapsToCumulative (laps: TimeSpan seq) =
    let mapping (state:TimeSpan) (lap:TimeSpan) =
        let total = lap + state
        (total, total)
    laps 
    |> Seq.mapFold mapping TimeSpan.Zero
    |> fst

let parseRow (row:resultsProvider.Smartiming.Row) =
    let name = sprintf "%s %s" row.``First Name`` row.Surname
    
    let laps = 
        [
            row.OutLap;
            row.Lap1;
            row.Lap2;
            row.Lap3;
            row.Lap4;
            row.Lap5;
            row.Lap6;
            row.Lap7;
        ] 
        |> Seq.filter (String.IsNullOrWhiteSpace >> not)
        |> Seq.map parseTimeSpan
        |> Seq.toList

    { 
        name = name;
        laps = laps;
        lapCount = laps.Length;
        cumulativeLapTime = laps |> lapsToCumulative |> Seq.toList
    }

table.Rows
|> Seq.map parseRow
|> Seq.filter (fun rider -> rider.name.Contains("Pridmore"))
|> Seq.iteri (printfn "%A %A")
