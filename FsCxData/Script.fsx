#r @"..\packages\FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"

#load "Library1.fs"
open FsCxData
open FSharp.Data

#if INTERACTIVE
System.IO.Directory.SetCurrentDirectory(__SOURCE_DIRECTORY__ )
#endif

let fileName = System.IO.Path.Combine(__SOURCE_DIRECTORY__,"RD_7_Data_small.txt")

//"RD_7_Data_small.txt" 
//|> System.IO.File.ReadLines
//|> Seq.map (fun row -> sprintf "%s" row)
//|> Seq.iter (printfn "%s")

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
    Sex: string
    ArmNumber: string
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
    }

CsvFile.Load(fileName).Cache().Rows
|> Seq.map csvRowToRider
|> Seq.take 1
|> Seq.iter (printfn "%A")

