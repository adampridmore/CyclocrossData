#r @"..\packages\FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"

#load "Library1.fs"
open FsCxData
open FSharp.Data
open FSharp.Charting
open System

#if INTERACTIVE
System.IO.Directory.SetCurrentDirectory(__SOURCE_DIRECTORY__ )
#endif

let filename = System.IO.Path.Combine(__SOURCE_DIRECTORY__,"RD_7_Data_small.txt")
//let filename = System.IO.Path.Combine(__SOURCE_DIRECTORY__,"RD_7_Data.txt")

//Bib
//First Name
//Surname
//Club
//Category
//Sex
//Arm Number
//Laps
//Lap1
//Lap2
//Lap3
//Lap4
//Lap5
//Lap6
//Lap7
//Lap8
//Lap9
//Lap10
//Finish
//Place Overall
//Place Category
//Place Sex

type Rider = {
    Chip: int;
    Forename: string;
    Surname: string;
    Club: string;
    Catagory: string;
    Sex: string;
    ArmNumber: string;
    LapCount: int;
    Laps : TimeSpan array;
    Finish: TimeSpan;
}

let readLaps cells = 
    let rec readLapRec (parsedLaps: TimeSpan list) (cells: String list) =
        let isTimeSpan (x:string) = x.Contains(":")  
        match cells with
        | [] -> parsedLaps
        | [hd] when hd |> isTimeSpan -> parsedLaps @ [TimeSpan.Parse(hd)]
        | hd::tail when hd |> isTimeSpan -> 
            parsedLaps @ [TimeSpan.Parse(hd)] @ (readLapRec parsedLaps tail)
        | _ -> parsedLaps
    
    readLapRec [] cells

readLaps ["00:10:00.0";"00:20:00.0"; "00:30:00.0"]
|> Seq.map (fun ts -> sprintf "%0.0f" ts.TotalSeconds )


let csvRowToRider (row:CsvRow) =
    let rawLaps =
        seq{0..row.Columns.Length-1}
        |> Seq.map (fun i -> row.[i])
        |> Seq.skip 8
        |> Seq.toList
        |> readLaps

    let laps = rawLaps |> Seq.take (rawLaps.Length) |> Seq.toArray
    let finish = rawLaps |> Seq.last 
    
    {
        Chip = row.[0].AsInteger();
        Forename = row.[1];
        Surname = row.[2];
        Club = row.[3];
        Catagory = row.[4];
        Sex = row.[5];
        ArmNumber = row.[6];
        LapCount = row.[7].AsInteger();
        Laps = laps;
        Finish = System.TimeSpan.Parse(row.[18])
    }

Chart.Line(
    CsvFile.Load(filename, hasHeaders = false).Rows
    |> Seq.map csvRowToRider
    |> Seq.take 100
    |> Seq.sortBy(fun r -> -r.LapCount, r.Finish)
    |> Seq.map(fun r->(r.Finish.TotalSeconds))
) |> Chart.Show


TimeSpan.Parse("03:56.8")
TimeSpan.ParseExact("03:56.8", "mm\:ss\.f", System.Globalization.CultureInfo.CurrentCulture)
