module HtmlParse

open FSharp.Data
open System
open Types
open TableTypes

let riderAndLapsFromHtml(url: String) : list<RiderRace> = 
    let data = url |> HtmlTable.LoadById "dt-user-list"

    let headers = data.columns

    let getColumnIndex name = 
        headers 
        |> Seq.findIndex (fun x -> x = name)

    let bibColumnIndex = "Bib" |> getColumnIndex
    let firstNameColumnIndex = "First Name" |> getColumnIndex
    let surnameColumnIndex = "Surname"|> getColumnIndex
    let clubColumnIndex = "Club" |> getColumnIndex
    let placeOverallIndex = "Place Overall" |> getColumnIndex

    let isLapColumnName (name:String) : bool = 
        match name with
        | name when name.Length <= 3 -> false
        | name when name = "Laps" -> false
        | name when name = "OutLap" -> true
        | name when name.Substring(0, 3) = "Lap" -> true
        | _ -> false

    let lapColumnIndexes : list<int> = 
        headers
        |> Seq.mapi(fun i name -> (i, name) ) 
        |> Seq.filter (snd >> isLapColumnName) 
        |> Seq.map fst
        |> Seq.toList

    let parseTimeSpan (text:string) : TimeSpan =
        try
            ("00:" + text) |> TimeSpan.Parse
        with | _ -> failwithf "Error parsing TimeSpan '%s'" text

    let rowToLaps (row : Row) : seq<TimeSpan> = 
        lapColumnIndexes 
        |> Seq.map (fun index -> row.values.[index])
        |> Seq.filter (String.IsNullOrWhiteSpace >> not)
        |> Seq.filter (fun v -> ["DNF";"DNS";"999"] |> Seq.contains v |> not)
        |> Seq.map parseTimeSpan

    let lapsToCumulative (laps: seq<TimeSpan>) : seq<TimeSpan> =
        let mapping (state:TimeSpan) (lap:TimeSpan) =
            let total = lap + state
            (total, total)
        laps 
        |> Seq.mapFold mapping TimeSpan.Zero
        |> fst

    let parseRow (row:Row) : RiderRace = 
        let firstname = row.values.[firstNameColumnIndex]
        let surname = row.values.[surnameColumnIndex]
        let club = row.values.[clubColumnIndex]
        let placeOverall = row.values.[placeOverallIndex] |> int32
        let bibNumber = row.values.[bibColumnIndex] |> int32

        let lapsTimes = row |> rowToLaps
        let cumulativeLaps = lapsTimes |> lapsToCumulative
    
        let laps = 
            Seq.zip lapsTimes cumulativeLaps 
            |> Seq.map (fun (l, cl) -> { lapTime = l ; cumulativeLapTime = cl})

        let name =
            match firstname with
            | "Unknown" -> (sprintf "Unknown Bib:%d" bibNumber)
            | _ -> (sprintf "%s %s" firstname surname)

        let rider = { name = name; club = club }

        { 
            rider = rider ;
            laps = laps |> Seq.toList ; 
            lapCount = laps |> Seq.length;
            placeOverall = placeOverall |> Some;
        }

    data.rows 
    |> Seq.map parseRow 
    |> Seq.filter (fun r -> r.rider.name |> String.IsNullOrWhiteSpace |> not)
    |> Seq.filter (fun r -> r.lapCount <> 0)
    |> Seq.sortBy (fun r -> r.rider.name)
    |> Seq.toList

 

