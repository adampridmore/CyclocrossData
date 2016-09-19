#r @"..\packages\FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"

#load "Types.fs"
#load "CsvParse.fs"

open FSharp.Data
open System
open CsvParse

data.Rows 
|> Seq.map parseCsvRow
