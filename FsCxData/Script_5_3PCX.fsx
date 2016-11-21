#r @"..\packages\FSharp.Data.2.3.2\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"
#r @"System.Windows.Forms.DataVisualization.dll"

#load "Types.fs"
//#load "HtmlParse.fs"
//#load "CsvParseYP.fs"
#load "CsvParse3PCX.fs"
#load "LapRender.fs"

open FSharp.Data
open FSharp.Charting
open System
//open HtmlParse
//open CsvParseYP
open CsvParse3PCX
open Types
open LapRender

let riderAndLaps = riderAndLapsFromCsv()

//riderAndLaps |> Seq.groupBy (fun r -> r.lapCount) |> Seq.map(fun (k,riders) -> (k, (riders |> Seq.length ) ) ) |> (printfn "%A")

let didAllSegments rider = rider.lapCount = 6

//riderAndLaps
//|> Seq.filter didAllSegments
////|> Seq.filter (fun ral -> ral.rider |> highlight)
////|> Seq.take 20
//|> render

riderAndLaps 
//|> mapToRiderAndLapPositions
|> Seq.iter (printfn ("%A"))

