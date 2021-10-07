#r "nuget: FSharp.Data, 4.2.3"
#r @"nuget:FSharp.Charting.Gtk, 2.1.0"
#r @"nuget: XPlot.GoogleCharts, 3.0.1"
//#r @"System.Windows.Forms.DataVisualization.dll"

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

let urls = [
//    @"http://results.smartiming.co.uk/view-race/ycca1617winterround1senior"
//    @"http://results.smartiming.co.uk/view-race/ycca1617winterround2senior"
//    @"http://results.smartiming.co.uk/view-race/ycca1617winterround3senior/" // RD3
//    @"http://results.smartiming.co.uk/view-race/ycca1617winterround4senior/" // RD4
//    @"http://results.smartiming.co.uk/view-race/ycca1617winterround5senior/" // RD5
//    @"http://results.smartiming.co.uk/view-race/ycca1617winterround5youth/" // RD5 - Youth
    @"http://results.smartiming.co.uk/view-race/ycca1617winterround8senior/"       // RD8
    ]

urls |> Seq.head |> Seq.singleton
//urls
|> Seq.map riderAndLapsFromHtml
|> Seq.iter render


//urls
//|> Seq.map riderAndLapsFromHtml



