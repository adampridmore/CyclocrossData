﻿module CsvParse3PCX

open FSharp.Data
open System
open Types

[<Literal>] 
let Filename = __SOURCE_DIRECTORY__ + @"\..\Data\ThreePeaks2016\Results.csv"

let data = CsvFile.Load(Filename)

let headers = data.Headers

let nameColumnIndex = headers.Value |> Seq.findIndex (fun x -> x = "Name")
let clubColumnIndex = headers.Value |> Seq.findIndex (fun x -> x = "Club")

let isSegmentColumnName (name:String) = 
    let segmentColumNames = ["Ingleborough";"Cold Cotes";"Whernside";"Ribblehead";"Pen-y-ghent";"Finish"]
    segmentColumNames |> Seq.contains name

let lapColumnIndexes = 
    headers.Value 
    |> Seq.mapi(fun i name -> (i, name) ) 
    |> Seq.filter (snd >> isSegmentColumnName) 
    |> Seq.map fst
    |> Seq.toList

let parseTimeSpan (text:string) =
    try
      text |> TimeSpan.Parse
    with | _ -> failwithf "Error parsing %s" text

let rowToLaps (row : CsvRow) = 
    lapColumnIndexes 
    |> Seq.map (fun index -> row.[index])
    |> Seq.filter (String.IsNullOrWhiteSpace >> not)
    |> Seq.filter (fun x -> x.StartsWith("-") |> not)
    |> Seq.map parseTimeSpan

let parseCsvRow (row:CsvRow) = 
    let name = row.Item(nameColumnIndex)
    let club = row.Item(clubColumnIndex)

    let lapsTimes = row |> rowToLaps
    let cumulativeLaps = lapsTimes 
    
    let laps = 
        lapsTimes 
        |> Seq.zip cumulativeLaps
        |> Seq.map (fun (l, cl) -> { lapTime = l ; cumulativeLapTime = cl})

    let rider = { name =  name; club = club}

    { 
        rider = rider ;
        laps = laps |> Seq.toList ; 
        lapCount = laps |> Seq.length
    }



let riderAndLapsFromCsv() = 
    data.Rows 
    |> Seq.map parseCsvRow



