#r @"..\packages\FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"

#load "Types.fs"
#load "HtmlParse.fs"
#load "CsvParse.fs"
#load "LapRender.fs"

open FSharp.Data
open FSharp.Charting
open System
open HtmlParse
open CsvParse
open Types
open LapRender

//let riderAndLaps = riderAndLapsFromHtml()
let riderAndLaps = riderAndLapsFromCsv()

riderAndLaps 
//|> Seq.filter (fun r -> r.rider.name = "Adam Pridmore" || r.rider.name = "Ian Cliffe") 
//|> Seq.take 10
//|> (printfn "%A")
|> render

//riderAndLaps |> render

//let maxLapsIndex =  riderAndLaps |> Seq.map (fun r -> r.lapCount) |> Seq.max |> minusOne
//    
//seq{0..maxLapsIndex}
//|> Seq.map (fun lapIndex -> riderAndLaps |> lapByRider lapIndex)
//|> Seq.collect id
//|> Seq.groupBy (fun r->r.rider)
//|> Seq.map (fun (r, rlp) -> r, (rlp |> Seq.map (fun x -> x.position) |> Seq.toList) )
//|> Seq.sortBy(fun (r , _) -> r.name)
////|> Seq.map (fun (rider, laps) -> toRiderChart rider laps)