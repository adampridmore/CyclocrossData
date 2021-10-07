#r "nuget: FSharp.Data, 4.2.3"
#r @"nuget:FSharp.Charting.Gtk, 2.1.0"
#r @"nuget: XPlot.GoogleCharts, 3.0.1"

#load "../Types.fs"
#load "../TableTypes.fs"
#load "../HtmlTable.fs"
#load "../HtmlParse.fs"
#load "../LapRender.fs"

open FSharp.Data
open FSharp.Charting
open System
open HtmlParse
open Types
open LapRender

// Winter 2021
let urls = [
//    @"http://results.smartiming.co.uk/view-race/yycard1mv40senior/"  // RD1 (Not on smart timings...)
    @"http://results.smartiming.co.uk/view-race/yycard2mv40senior/"; // RD2
    @"http://results.smartiming.co.uk/view-race/yycard3mv40senior/"  // RD3
    ]


urls //|> Seq.head |> Seq.singleton
|> Seq.map riderAndLapsFromHtml
|> Seq.iter render



