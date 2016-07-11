#r @"..\packages\FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"

#load "Library1.fs"
open FsCxData
open FSharp.Data
open FSharp.Charting

#if INTERACTIVE
System.IO.Directory.SetCurrentDirectory(__SOURCE_DIRECTORY__ )
#endif

//let filename = System.IO.Path.Combine(__SOURCE_DIRECTORY__,"RD_7_Data_small.txt")
let filename = System.IO.Path.Combine(__SOURCE_DIRECTORY__,"RD_7_Data.txt")

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
    Finish: System.TimeSpan
}

let csvRowToRider (row:CsvRow) = 
    { 
        Chip = row.[0].AsInteger();
        Forename = row.[1];
        Surname = row.[2];
        Club = row.[3];
        Catagory = row.[4];
        Sex = row.[5];
        ArmNumber = row.[6];
        LapCount = row.[7].AsInteger();

        Finish = System.TimeSpan.Parse(row.[18])
    }

Chart.Line(
    CsvFile.Load(filename, hasHeaders = false).Rows
    |> Seq.map csvRowToRider
    |> Seq.take 100
    |> Seq.sortBy(fun r -> -r.Laps, r.Finish)
    |> Seq.map(fun r->(r.Finish.TotalSeconds))
) |> Chart.Show


