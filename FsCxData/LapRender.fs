﻿module LapRender

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

let startsWith (value:string) (name:string) = 
    name.StartsWith(value)

//let highlightByClub (rider:Rider) = 
//    rider.club.ToLower().Contains("Ilkley".ToLower())

let highlightByRiderList (rider:Rider) = 
    let highlighList = [
        "Adam Pridmore"; 
        "Ian Cliffe"; 
        "Simon Everet";
        "Stuart Hull";
        "Michael Burdon";
        "John Elwell";
        "John Wilkinson";
        "Richard Brewster";
        "Paul Oldham";
        "Rob Jebb";
        "Nick Craig";
        "Ian Taylor"
    ]
    
    match rider.name with
    | name when highlighList |> Seq.exists (fun n -> name.StartsWith(n)) -> true
    | _ -> false

let highlight = fun _ -> false

let toRiderChart rider (laps: int list) (color: System.Drawing.Color) = 
    let name = sprintf "%s(%d)" rider.name (laps |> Seq.last) 

    let borderWidth = 5
    
    FSharp.Charting.Chart.Line(laps, Name = name)
    |> FSharp.Charting.Chart.WithStyling(BorderWidth=borderWidth) 

let numberToColor i = 
    let randomColor = Color.FromArgb(i * 100)
    Color.FromArgb(255, randomColor)

let getCharForRiderAndLapPositions riderAndLaps =
    let maximumSegmentIndex = 
        (riderAndLaps 
        |> Seq.map snd 
        |> Seq.map Seq.length
        |> Seq.max) - 1
    
    riderAndLaps
    |> Seq.mapi (fun i (rider, laps) -> toRiderChart rider laps (i |> numberToColor) )
    |> Chart.Combine
    |> Chart.WithXAxis(Title="Lap")
    |> Chart.WithYAxis(Title="Position")
    |> Chart.WithArea.AxisX(Minimum=0.0,Maximum=(maximumSegmentIndex |> float))
    |> Chart.WithLegend(Enabled=true,Title="Riders", InsideArea=false )
    
let render (riderAndLaps : RiderRace seq) = 
    riderAndLaps 
    |> mapToRiderAndLapPositions
    |> Seq.sortBy (fun (rider,_) -> rider |> highlight)
    |> getCharForRiderAndLapPositions
    |> Chart.Show

