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

let toRiderChart rider (laps: int list) (color: System.Drawing.Color) = 
    let name = sprintf "%s(%d)" rider.name (laps |> Seq.last) 

    FSharp.Charting.Chart.Line(laps, Name = name, (* Color = color,*) XTitle = "Lap", YTitle = "Position")
    |> FSharp.Charting.Chart.WithStyling(BorderWidth=6) 


let numberToColor i = 
    let randomColor = Color.FromArgb(i * 100)
    Color.FromArgb(255, randomColor)

let showCharForRiderAndLapPositions riderAndLaps =
    riderAndLaps
    |> Seq.mapi (fun i (rider, laps) -> toRiderChart rider laps (i |> numberToColor) )
    |> Chart.Combine
    //|> Chart.WithLegend(Enabled=true,Title="Riders", InsideArea=false, Docking=ChartTypes.Docking.Bottom)
    |> Chart.WithLegend(Enabled=true,Title="Riders", InsideArea=false )
    //|> Chart.WithLegend(Enabled=true,Title="Riders")
    |> Chart.Show 

let render riderAndLaps = 
    riderAndLaps 
    |> mapToRiderAndLapPositions
    |> showCharForRiderAndLapPositions


