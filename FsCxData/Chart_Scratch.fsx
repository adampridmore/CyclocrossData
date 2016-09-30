#r @"..\packages\FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"
#r @"System.Windows.Forms.DataVisualization.dll"

open FSharp.Data
open FSharp.Charting
open System

let fn x = System.Math.Sin (x * Math.PI)
let fn2 x = System.Math.Cos(x * Math.PI)

let minX = -2.0
let maxX = 2.0
let minY = -1.5
let maxY = 1.5

let generateData f = 
    seq{minX..0.01..maxX}
    |> Seq.map (fun x -> x, f(x) )

[
    Chart.Line (fn |> generateData, XTitle = "π", YTitle = "sin (π)", Name="y=fn(x)")
    Chart.Line (fn2 |> generateData, XTitle = "π", YTitle = "cos (π)", Name="y=fn2(x)")
] 
|> Chart.Combine
|> Chart.WithLegend()
|> Chart.WithTitle ("sin and cos functions",InsideArea=false)
|> Chart.WithXAxis(Min = minX, Max= maxX, TitleFontSize=20.0, Title="x (π)")
|> Chart.WithYAxis(Min = minY, Max= maxY, TitleFontSize=20.0, Title="fn(x)")
|> Chart.Show



let generateDataForSeries (index:int) = 
    seq{0..10}
    |> Seq.map (fun x -> match x with 
                         | 0 -> 0
                         | 1 -> 1
                         | _ -> x + index)

let chart = 
    seq{0..50}
    |> Seq.map generateDataForSeries
    |> Seq.map (fun data -> Chart.Line data)
    |> Seq.rev
    |> Chart.Combine
    |> Chart.WithLegend()

let chartControl = new FSharp.Charting.ChartTypes.ChartControl(chart);


chart |> Chart.WithStyling()

chart |> Chart.Show

chart |> Chart.Save("c:\Temp\CX\size.png")