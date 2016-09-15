#r @"..\packages\FSharp.Data.2.3.1\lib\net40\FSharp.Data.dll"
#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"

#load "Library1.fs"
#load "HtmlParse.fs"

open FsCxData
open FSharp.Data
open FSharp.Charting
open System
open HtmlParse

let riders = table.Rows |> Seq.map parseRow |> Seq.toList

type RiderLapPosition = {
    rider : Rider;
    position : int
}

let maxLapCount = riders |> Seq.map (fun r->r.lapCount) |> Seq.max

let toLapPositions lapNumber = 
    riders 
    |> Seq.filter (fun r -> r.laps.Length > lapNumber-1)
    |> Seq.sortBy (fun r -> r.laps.[lapNumber-1].cumulativeLapTime)
    |> Seq.mapi (fun i r -> {rider=r;position=i+1})
    |> Seq.toList
    
let lapPositions = 
    seq{1..maxLapCount}
    |> Seq.map toLapPositions
    |> Seq.toList



let getRiderPositionForLap (rider:Rider) (riderLapPositions : RiderLapPosition seq) = 
    riderLapPositions 
    |> Seq.tryFind (fun (rlp:RiderLapPosition) -> rlp.rider = rider)
    |> function
        | Some(x) -> Some(x.position)
        | _ -> None


let riderToLapPositions (rider:Rider) = 
    rider.laps 
    |> Seq.mapi (fun lapIndex _ -> getRiderPositionForLap rider lapPositions )

    |> Seq.mapi (fun lapIndex _ -> lapPositions.[lapIndex] |> Seq.find(fun (riderLapPosition: RiderLapPosition) -> riderLapPosition.rider = rider ) )
    |> Seq.map (fun rlp -> rlp.position)
    |> Seq.toList
    
let positionsToString (positions: int seq) = 
    positions 
    |> Seq.map (fun p->p.ToString())
    |> Seq.reduce (sprintf "%s %s")

riders 
|> Seq.map (fun rider -> (rider, rider |> riderToLapPositions))
|> Seq.iter (fun (rider, positions) -> printfn "%s - %s" rider.name (positions |> positionsToString))




riders
|> Seq.map (fun (rider:Rider) -> rider.laps |> Seq.map (fun lap -> lap.cumulativeLapTime.TotalSeconds))
|> Seq.map FSharp.Charting.Chart.Line
|> FSharp.Charting.Chart.Combine
|> FSharp.Charting.Chart.Show

// TODO - Calculate lap position
