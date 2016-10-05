#r @"..\packages\FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"
#r @"System.Windows.Forms.DataVisualization.dll"

#load "Types.fs"
#load "TableTypes.fs"
#load "HtmlTable.fs"
#load "HtmlParse.fs"
#load "LapRender.fs"

open FSharp.Data
open FSharp.Charting
open System
open HtmlParse
open Types
open LapRender

let riderAndLaps = riderAndLapsFromHtml()

riderAndLaps
//|> Seq.filter (fun ral -> ral.rider |> highlight)
//|> Seq.take 20
|> render
