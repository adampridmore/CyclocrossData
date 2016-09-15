module Types

open System

type LapColumn = { 
    index: int; 
    lapNumber: int
}  

[<StructuredFormatDisplay("LapTime {lapTime}, cumulativeLapTime {cumulativeLapTime}")>]
type Lap = {
    lapTime : TimeSpan
    cumulativeLapTime : TimeSpan
}

type Rider = {
    name: string
}

type RiderAndLaps = {
    rider: Rider
    laps : Lap list
    lapCount : int
}
