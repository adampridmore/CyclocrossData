module Types

open System

[<StructuredFormatDisplay("LapTime {lapTime}, cumulativeLapTime {cumulativeLapTime}")>]
type Lap = {
    lapTime : TimeSpan
    cumulativeLapTime : TimeSpan
}

type Rider = {
    name: string;
    club: string
}

[<StructuredFormatDisplay("RiderAndLaps {rider}, lapCount {lapCount}")>]
type RiderAndLaps = {
    rider: Rider
    laps : Lap list
    lapCount : int
}