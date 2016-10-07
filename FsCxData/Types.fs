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

[<StructuredFormatDisplay("RiderRace {rider}, lapCount {lapCount}, placeOverall {placeOverall}")>]
type RiderRace = {
    rider: Rider
    laps : Lap list
    lapCount : int;
    placeOverall : int Option;
}