module LapRender

open System.Drawing
open FSharp.Data
open FSharp.Charting
open System
open Types

type RiderLapPosition = {
    rider: Rider;
    position: int;
    lapNumber: int    
}

let lapByRider lapNumber riders = 
    riders 
    |> Seq.filter (fun r -> (r.lapCount - 1 ) >= lapNumber) 
    |> Seq.sortBy (fun r -> r.laps.[lapNumber].cumulativeLapTime)
    |> Seq.mapi (fun i r -> {rider = r.rider ; position = i; lapNumber = lapNumber})
    |> Seq.toList

let minusOne x = x - 1

let maxLapsIndex riderAndLaps = 
    riderAndLaps |> Seq.map (fun r -> r.lapCount) |> Seq.max |> minusOne

let getFinalPosition laps = 
    (laps |> Seq.last)

let mapToRiderAndLapPositions riderAndLaps = 
    let maxLapsIndex =  riderAndLaps |> Seq.map (fun r -> r.lapCount) |> Seq.max |> minusOne

    seq{0..maxLapsIndex}
    |> Seq.map (fun lapIndex -> riderAndLaps |> lapByRider lapIndex)
    |> Seq.collect id
    |> Seq.groupBy (fun r->r.rider)
    |> Seq.map (fun (r, rlp) -> r, (rlp |> Seq.map (fun x -> x.position) |> Seq.toList) )
    |> Seq.sortBy(fun (r , _) -> r.name)

let toRiderChartLinePoints rider (laps: int list) (color: System.Drawing.Color) : seq<int * int> = 
    let name = sprintf "%s(%d)" rider.name (laps |> Seq.last) 

    let borderWidth = 5
    
    laps |> Seq.mapi(fun i lap -> (i, lap))
    
let numberToColor i = 
    let randomColor = Color.FromArgb(i * 100)
    Color.FromArgb(255, randomColor)
 
let getCharForRiderAndLapPositions (riderAndLaps: seq<Rider * int list>): XPlot.GoogleCharts.GoogleChart =        
    let labels = riderAndLaps |> Seq.map(fun rl -> (fst rl).name )
    
    riderAndLaps
    |> Seq.mapi (fun i (rider, laps) -> toRiderChartLinePoints rider laps (i |> numberToColor) )
    |> XPlot.GoogleCharts.Chart.Line
    |> XPlot.GoogleCharts.Chart.WithLabels(labels)
    |> XPlot.GoogleCharts.Chart.WithXTitle("Lap")
    |> XPlot.GoogleCharts.Chart.WithYTitle("Position")
    |> XPlot.GoogleCharts.Chart.WithSize(1024, 2048)
   
let toGoogleChart(riderAndLaps: RiderRace seq) : XPlot.GoogleCharts.GoogleChart = 
    riderAndLaps 
    |> mapToRiderAndLapPositions
    |> Seq.sortBy fst
    |> getCharForRiderAndLapPositions

let render (riderAndLaps : RiderRace seq) : Unit = 
    let googleChart : XPlot.GoogleCharts.GoogleChart =
        riderAndLaps 
        |> toGoogleChart

    googleChart.Show()
