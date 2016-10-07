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

let urls = [
    @"http://results.smartiming.co.uk/view-race/ycca1617winterround1senior"
    @"http://results.smartiming.co.uk/view-race/ycca1617winterround2senior"
    @"http://results.smartiming.co.uk/view-race/ycca1617winterround3senior/" // RD3
    @"http://results.smartiming.co.uk/view-race/ycca1617winterround4senior/" // RD4
    ]

urls.[3] |> Seq.singleton
//urls
|> Seq.map riderAndLapsFromHtml
|> Seq.iter render


//urls
//|> Seq.map riderAndLapsFromHtml



