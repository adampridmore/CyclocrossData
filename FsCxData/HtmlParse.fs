﻿module HtmlParse

open FSharp.Data
open System

[<Literal>] 
let Url = @"http://results.smartiming.co.uk/view-race/ycca1617winterround2senior"
//let url = "..\..\..\CX\Html\Race Times.html"
//let url = @"C:\Users\Adam\Dropbox\Work\Dev\Cyclocross\CX\Html\Race Times.html"

type resultsProvider = HtmlProvider<Url>

type LapColumn = { 
    index: int; 
    lapNumber: int
}  

type Lap = {
    lapTime : TimeSpan
    cumulativeLapTime : TimeSpan
}

type Rider = {
    name: string
    laps : Lap list
    lapCount : int
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
    
    let lapTimes = 
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

    let cumulativeLapTimes = lapTimes |> lapsToCumulative |> Seq.toList

    let laps = 
        Seq.zip lapTimes cumulativeLapTimes
        |> Seq.map (fun (lapTime,cumulativeLapTime) -> { lapTime = lapTime; cumulativeLapTime = cumulativeLapTime})
        |> Seq.toList

    { 
        name = name;
        laps = laps;
        lapCount = laps.Length;
    }