#r @"..\packages\FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"

#load "Library1.fs"
#load "HtmlParse.fs"

open FsCxData
open FSharp.Data
open FSharp.Charting
open System
open HtmlParse

let riders = table.Rows |> Seq.map parseRow

riders
|> Seq.map (fun (rider:Rider) -> rider.laps |> Seq.map (fun lap -> lap.cumulativeLapTime.TotalSeconds))
|> Seq.map FSharp.Charting.Chart.Line
|> FSharp.Charting.Chart.Combine
|> FSharp.Charting.Chart.Show

// TODO - Calculate lap position
