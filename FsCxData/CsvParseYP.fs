module CsvParseYP

open FSharp.Data
open System
open Types

[<Literal>] 
//let Filename =  __SOURCE_DIRECTORY__ + @"\..\..\Data\2\rd3.csv"
//let Filename =  __SOURCE_DIRECTORY__ + @"\..\..\Data\2\rd3_top10.csv"

//let Filename = __SOURCE_DIRECTORY__ + @"\..\..\Data\RacesWinter2016\RD1.csv"
//let Filename = __SOURCE_DIRECTORY__ + @"\..\..\Data\RacesWinter2016\RD2.csv"
let Filename = __SOURCE_DIRECTORY__ + @"\..\Data\RacesWinter2016\RD3.csv"
//let Filename = @"C:\Users\Adam\Desktop\Temp\ThreePeaksCX\Results.csv"

let data = CsvFile.Load(Filename) //.Cache()

let headers = data.Headers

let firstNameColumnIndex = headers.Value |> Seq.findIndex (fun x -> x = "First Name")
let surnameColumnIndex = headers.Value |> Seq.findIndex (fun x -> x = "Surname")
let clubColumnIndex = headers.Value |> Seq.findIndex (fun x -> x = "Club")
let placeOverallColumnIndex = headers.Value |> Seq.findIndex (fun x -> x = "Place Overall")

let isLapColumnName (name:String) = 
    match (name.ToLower()) with
    | name when name.Length <= 3 -> false
    | name when name = "laps" -> false
    | name when name = "outlap" -> true
    | name when name.Substring(0, 3) = "lap" -> true
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
    let club = row.Item(clubColumnIndex)
    let placeOverall = row.Item(placeOverallColumnIndex) |> int32 |> Some
    
    let lapsTimes = row |> rowToLaps
    let cumulativeLaps = lapsTimes |> lapsToCumulative

    
    let laps = 
        lapsTimes 
        |> Seq.zip cumulativeLaps
        |> Seq.map (fun (l, cl) -> { lapTime = l ; cumulativeLapTime = cl})

    let rider = { name = (sprintf "%s %s" firstname surname); club = club }

    { 
        rider = rider ;
        laps = laps |> Seq.toList ; 
        lapCount = laps |> Seq.length;
        placeOverall= placeOverall;
    }



let riderAndLapsFromCsv() = 
    data.Rows 
    |> Seq.map parseCsvRow



