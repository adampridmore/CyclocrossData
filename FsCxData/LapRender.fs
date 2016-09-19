module LapRender

open FSharp.Data
open FSharp.Charting
open System
open Types

//let riderAndLaps = 
//    table.Rows 
//    |> Seq.map parseRow 
//    //|> Seq.take 5 
//    |> Seq.filter (fun r -> r.rider.name |> String.IsNullOrWhiteSpace |> not)
//    |> Seq.toList

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

let toRiderChart rider (laps: int list) = 
    FSharp.Charting.Chart.Line(laps, Name=(sprintf "%s(%d)" rider.name (laps |> Seq.last) ) ) 
    |> FSharp.Charting.Chart.WithStyling(BorderWidth=4)

let mapToRiderAndLapPositions riderAndLaps = 
    let maxLapsIndex =  riderAndLaps |> Seq.map (fun r -> r.lapCount) |> Seq.max |> minusOne

    seq{0..maxLapsIndex}
    |> Seq.map (fun lapIndex -> riderAndLaps |> lapByRider lapIndex)
    |> Seq.collect id
    |> Seq.groupBy (fun r->r.rider)
    |> Seq.map (fun (r, rlp) -> r, (rlp |> Seq.map (fun x -> x.position) |> Seq.toList) )
    |> Seq.sortBy(fun (r , _) -> r.name)

let showCharForRiderAndLapPositions riderAndLaps =
    riderAndLaps
    |> Seq.map (fun (rider, laps) -> toRiderChart rider laps)
    |> Chart.Combine 
    //|> Chart.WithLegend(Enabled=true,Title="Riders", InsideArea=false, Docking=ChartTypes.Docking.Bottom)
    |> Chart.WithLegend(Enabled=true,Title="Riders", InsideArea=false )
    //|> Chart.WithLegend(Enabled=true,Title="Riders")
    |> Chart.Show 

let render riderAndLaps = 
    riderAndLaps 
    |> mapToRiderAndLapPositions
    |> showCharForRiderAndLapPositions
        