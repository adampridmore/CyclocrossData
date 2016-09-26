﻿module CsvParse

open FSharp.Data
open System
open Types

[<Literal>] 
//let Filename =  __SOURCE_DIRECTORY__ + @"\..\..\CX\2\rd3.csv"
//let Filename =  __SOURCE_DIRECTORY__ + @"\..\..\CX\2\rd3_top10.csv"

//let Filename = __SOURCE_DIRECTORY__ + @"\..\..\CX\RacesWinter2016\RD1.csv"
//let Filename = __SOURCE_DIRECTORY__ + @"\..\..\CX\RacesWinter2016\RD2.csv"
let Filename = __SOURCE_DIRECTORY__ + @"\..\..\CX\RacesWinter2016\RD3.csv"
//let Filename = @"C:\Users\Adam\Desktop\Temp\ThreePeaksCX\Results.csv"

let data = CsvFile.Load(Filename) //.Cache()

let headers = data.Headers

let firstNameColumnIndex = headers.Value |> Seq.findIndex (fun x -> x = "First Name")
let surnameColumnIndex = headers.Value |> Seq.findIndex (fun x -> x = "Surname")

let isLapColumnName (name:String) = 
    match name with
    | name when name.Length <= 3 -> false
    | name when name = "Laps" -> false
    | name when name = "OutLap" -> true
    | name when name.Substring(0, 3) = "Lap" -> true
    | _ -> false

let lapColumnIndexes = 
    headers.Value 
    |> Seq.mapi(fun i name -> (i, name) ) 
    |> Seq.filter (snd >> isLapColumnName) 
    |> Seq.map fst
    |> Seq.toList

let parseTimeSpan (text:string) =
    try
        ("00:" + text) |> TimeSpan.Parse
    with | _ -> failwithf "Error parsing %s" text

let rowToLaps (row : CsvRow) = 
    lapColumnIndexes 
    |> Seq.map (fun index -> row.[index])
    |> Seq.filter (String.IsNullOrWhiteSpace >> not)
    |> Seq.map parseTimeSpan

let lapsToCumulative (laps: TimeSpan seq) =
    let mapping (state:TimeSpan) (lap:TimeSpan) =
        let total = lap + state
        (total, total)
    laps 
    |> Seq.mapFold mapping TimeSpan.Zero
    |> fst

let parseCsvRow (row:CsvRow) = 
    let firstname = row.Item(firstNameColumnIndex)
    let surname = row.Item(surnameColumnIndex)
    
    let lapsTimes = row |> rowToLaps
    let cumulativeLaps = lapsTimes |> lapsToCumulative
    
    let laps = 
        lapsTimes 
        |> Seq.zip cumulativeLaps
        |> Seq.map (fun (l, cl) -> { lapTime = l ; cumulativeLapTime = cl})

    let rider = { name = (sprintf "%s %s" firstname surname) }

    { 
        rider = rider ;
        laps = laps |> Seq.toList ; 
        lapCount = laps |> Seq.length
    }



let riderAndLapsFromCsv() = 
    data.Rows 
    |> Seq.map parseCsvRow


