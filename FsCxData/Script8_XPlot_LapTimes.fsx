#r @"..\packages\XPlot.GoogleCharts.1.4.2\lib\net45\XPlot.GoogleCharts.dll"
#r @"..\packages\Google.DataTable.Net.Wrapper.3.1.2.0\lib\Google.DataTable.Net.Wrapper.dll"
#r @"..\packages\Newtonsoft.Json.9.0.1\lib\net45\Newtonsoft.Json.dll"

#r @"..\packages\FSharp.Data.2.3.2\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"
#r @"System.Windows.Forms.DataVisualization.dll"

#load "Types.fs"
#load "TableTypes.fs"
#load "HtmlTable.fs"
#load "HtmlParse.fs"
#load "LapRender.fs"

open System
open HtmlParse
open Types
open LapRender

open XPlot.GoogleCharts

let title ="YP Seniors & V40's Lap Chart for RD8 Shibden Park"
let url = @"http://results.smartiming.co.uk/view-race/ycca1617winterround8senior/"       // RD8 Senior + V40
let maxLaps = 10
let maxPositions = 8

let riderAndLapPositions = 
    url
    |> riderAndLapsFromHtml
    
let data = 
    riderAndLapPositions 
    |> Seq.map (fun rider -> rider, (rider.laps |> Seq.mapi (fun lapIndex lap -> lap.cumulativeLapTime.TotalSeconds, lapIndex) ) )
    |> Seq.toList
   
let printRider name (laps:seq<int*float>) = 
    printfn "%s" name
    laps 
    |> Seq.iter (fun (lap, seconds) -> printfn "%d (%fd)" lap seconds )

let hAxis = Axis(title = "Lap", gridlines= Gridlines(count=maxLaps))
let vAxis = Axis(title = "Cumulative Seconds", gridlines = Gridlines(count=maxPositions))

let labels = 
    riderAndLapPositions 
    |> Seq.map (fun rider -> rider.rider.name)

let options =
    Options( 
        title = title, 
        legend = Legend(position = "right"),
        height = 2000, 
        width = 1600,
        hAxis = hAxis, 
        vAxis = vAxis,
        lineWidth = 3
    )

data 
|> Seq.map snd
|> XPlot.GoogleCharts.Chart.Line
|> Chart.WithLabels labels
|> Chart.WithOptions options
|> Chart.Show