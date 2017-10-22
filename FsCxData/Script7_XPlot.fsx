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

//let labels = ["Dave"; "Fred"] 
//
//let data2 = [
//    [(1,1);(2,1);(3,2)]
//    [(1,2);(2,2);(3,1)];
//]
//
//let options =
//    Options( 
//        title = "CX Race Lap Chart", 
//        legend = Legend(position = "right") 
//    )
//
//data2
//|> XPlot.GoogleCharts.Chart.Line
//|> Chart.WithLabels labels
//|> Chart.WithXTitle "Lap"
//|> Chart.WithYTitle "Position"
//|> Chart.WithOptions options
////|> Chart.WithHeight 1000
//|> Chart.Show

//let title ="YP Seniors & V40's Lap Chart for RD8 Shibden Park"
//let url = @"http://results.smartiming.co.uk/view-race/ycca1617winterround8senior/"       // RD8 Senion + V40
//let maxLaps = 10
//let maxPositions = 8

//let title ="YP Female & V45+ Lap Chart for RD8 Shibden Park"
//let url = @"http://results.smartiming.co.uk/view-race/ycca1617winterround8v50andwomen/"       // RD8 - V50 + FM
//let maxLaps = 6
//let maxPositions = 9



//let title ="North of England Championships Seniors Lap Chart (York)"
//let url = @"http://results.smartiming.co.uk/view-race/nofenglandchamps2016seniors/"
//let maxLaps = 9
//let maxPositions = 10

//let title ="North of England Championships V40 Junior Lap Chart (York)"
//let url = @"http://results.smartiming.co.uk/view-race/nofenglandchamps2016v40andjuniors/"
//let maxLaps = 6
//let maxPositions = 10

//let title ="Ilkely Cyclocross 2017 - Seniors & V40's Lap Chart"
//let url = @"http://results.smartiming.co.uk/view-race/ilkleysenior/"
//let maxLaps = 5
//let maxPositions = 10

//let title ="Wentworth Castle Cyclocross 2017 - Rd9 - Seniors & V40's Lap Chart"
//let url = @"http://results.smartiming.co.uk/view-race/ycca1617winterround9senior/"
//let maxLaps = 5
//let maxPositions = 10

//let title ="Yorkshire Points Summer Cyclocross 2017 - Seniors & V40's Time Chart"
//let url = @"http://results.smartiming.co.uk/view-race/yccasummer2017rd1over14/"
//let maxLaps = 5
//let maxPositions = 10

//let title ="Yorkshire Points Summer Cyclocross 2017 Rd4 - Seniors & V40's Time Chart"
//let url = @"http://results.smartiming.co.uk/view-race/yccasummer2017rd4over14/"
//let maxLaps = 5
//let maxPositions = 10

//let title ="Yorkshire Points Summer Cyclocross 2017 Rd5 Wakefield - Seniors & V40's Time Chart"
//let url = @"http://results.smartiming.co.uk/view-race/yccasummer2017rd5over14/"
//let maxLaps = 5
//let maxPositions = 10

//let title ="Yorkshire Points Summer Cyclocross 2017 Otley - Juniors & Seniors Time Chart"
//let url = @"http://results.smartiming.co.uk/view-race/yccasummer2017otleysenjunmen/"
//let maxLaps = 5
//let maxPositions = 14

//let title ="Yorkshire Points Summer Cyclocross 2017 Otley - Vet & Womens"
//let url = @"http://results.smartiming.co.uk/view-race/yccasummer2017otleyvetwomen/"
//let maxLaps = 5
//let maxPositions = 14

//let title ="Yorkshire Points Winter Cyclocross 2017 Rd2 - Temple Newsham - SEN + V40"
//let url = @"http://results.smartiming.co.uk/view-race/ycca1718winterseriesr2senv40/"
//let maxLaps = 5
//let maxPositions = 14

//let title ="Yorkshire Points Winter Cyclocross 2017 Rd3 - Bedale - SEN + V40"
//let url = @"http://results.smartiming.co.uk/view-race/ycca1718winterseriesr3senv40/"
//let maxLaps = 5
//let maxPositions = 14

//let title ="Yorkshire Points Winter Cyclocross 2017 Rd4 - Skipton - SEN + V40"
//let url = @"http://results.smartiming.co.uk/view-race/ycca1718winterseriesr4senv40/"
//let maxLaps = 5
//let maxPositions = 14

//let title ="Yorkshire Points Winter Cyclocross 2017 Rd5 - Huddersfield - SEN + V40"
//let url = @"http://results.smartiming.co.uk/view-race/ycca1718winterseriesr5senv40/"
//let maxLaps = 5
//let maxPositions = 14


let title ="Rd6 - Yorkshire Points Winter Cyclocross 2017 - Halifax- SEN + V40"
let url = @"http://results.smartiming.co.uk/view-race/ycca1718winterseriesr6senv40/"
let maxLaps = 5
let maxPositions = 14




let riderAndLapPositions = 
    url
    |> riderAndLapsFromHtml
    |> mapToRiderAndLapPositions

let lapPositionToOneOffset x = x + 1
let lapPositionToOneOffset2 x = x + 1

let data = 
    riderAndLapPositions
    |> Seq.map (fun (rider, laps) -> laps |> Seq.mapi(fun i lapPosition -> (i,lapPosition |> lapPositionToOneOffset)))

let labels = 
    let riderToName (rider, lapPosition) =
        sprintf "%s (%d)" rider.name (lapPosition |> Seq.last |> lapPositionToOneOffset)
    
    riderAndLapPositions 
    |> Seq.map riderToName
    
let hAxis = Axis(title = "Lap", gridlines= Gridlines(count=maxLaps))
let vAxis = Axis(title = "Position", gridlines = Gridlines(count=maxPositions))

let options =
    Options( 
        title = title, 
        legend = Legend(position = "right"),
        height = 1600, 
        width = 1200,
        hAxis = hAxis, 
        vAxis = vAxis,
        lineWidth = 5
    )

let setChartId title (chart:GoogleChart) =
    chart.Id <- title  
    chart
      

data 
|> XPlot.GoogleCharts.Chart.Line 
|> setChartId title
|> Chart.WithLabels labels
|> Chart.WithOptions options
|> Chart.Show
