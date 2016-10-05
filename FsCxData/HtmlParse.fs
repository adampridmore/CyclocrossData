module HtmlParse

open FSharp.Data
open System
open Types
open TableTypes

[<Literal>] 
// let Url = @"http://results.smartiming.co.uk/view-race/ycca1617winterround1senior"
let Url = @"http://results.smartiming.co.uk/view-race/ycca1617winterround2senior"
// let Url = @"http://results.smartiming.co.uk/view-race/ycca1617winterround3senior/" // RD3
// let Url = @"http://results.smartiming.co.uk/view-race/ycca1617winterround4senior/" // RD4

let data = Url |> HtmlTable.LoadById "dt-user-list"

let headers = data.columns

let firstNameColumnIndex = headers |> Seq.findIndex (fun x -> x = "First Name")
let surnameColumnIndex = headers |> Seq.findIndex (fun x -> x = "Surname")
let clubColumnIndex = headers |> Seq.findIndex (fun x -> x = "Club")

let isLapColumnName (name:String) = 
    match name with
    | name when name.Length <= 3 -> false
    | name when name = "Laps" -> false
    | name when name = "OutLap" -> true
    | name when name.Substring(0, 3) = "Lap" -> true
    | _ -> false

let lapColumnIndexes = 
    headers
    |> Seq.mapi(fun i name -> (i, name) ) 
    |> Seq.filter (snd >> isLapColumnName) 
    |> Seq.map fst
    |> Seq.toList

let parseTimeSpan (text:string) =
    try
        ("00:" + text) |> TimeSpan.Parse
    with | _ -> failwithf "Error parsing %s" text

let rowToLaps (row : Row) = 
    lapColumnIndexes 
    |> Seq.map (fun index -> row.values.[index])
    |> Seq.filter (String.IsNullOrWhiteSpace >> not)
    |> Seq.map parseTimeSpan

let lapsToCumulative (laps: TimeSpan seq) =
    let mapping (state:TimeSpan) (lap:TimeSpan) =
        let total = lap + state
        (total, total)
    laps 
    |> Seq.mapFold mapping TimeSpan.Zero
    |> fst

let parseRow (row:Row) = 
    let firstname = row.values.[firstNameColumnIndex]
    let surname = row.values.[surnameColumnIndex]
    let club = row.values.[clubColumnIndex]
    
    let lapsTimes = row |> rowToLaps
    let cumulativeLaps = lapsTimes |> lapsToCumulative
    
    let laps = 
        Seq.zip lapsTimes cumulativeLaps 
        |> Seq.map (fun (l, cl) -> { lapTime = l ; cumulativeLapTime = cl})

    let rider = { name = (sprintf "%s %s" firstname surname); club = club }

    { 
        rider = rider ;
        laps = laps |> Seq.toList ; 
        lapCount = laps |> Seq.length
    }

let riderAndLapsFromHtml() = 
    data.rows 
    |> Seq.map parseRow 
//    |> Seq.take 1
    |> Seq.filter (fun r -> r.rider.name |> String.IsNullOrWhiteSpace |> not)
    |> Seq.toList

 

