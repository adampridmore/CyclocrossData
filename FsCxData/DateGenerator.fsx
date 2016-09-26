open System

let incrementingSeq increment initialVal = 
    Seq.unfold (fun v -> Some(v, v + increment )) initialVal

let blockSeq incrementSize initialValue endValue = 
    incrementingSeq incrementSize initialValue
    |> Seq.pairwise
    |> Seq.takeWhile (fun (a,_ )-> a < endValue)
    |> Seq.map (fun (a, b) -> (a, min b endValue ) )

DateTime.Now 
|> incrementingSeq (TimeSpan.FromMinutes(15.0)) 
|> Seq.take 100
|> Seq.iter (printfn "%A")

//10 |> incrementingSeq 1 |> Seq.take 10 |>  Seq.iter (printfn "%A")



let startTime = new DateTime(2001,1,1)
let endTime = (new DateTime(2001,1,10))
let incrementSize = (TimeSpan.FromDays(1.0) )

blockSeq incrementSize startTime endTime
|> Seq.iter (printfn "%A")

let startTime2 = new DateTime(2001,1,1)
let endTime2 = (new DateTime(2001,1,4))
let incrementSize2 = (TimeSpan.FromDays(2.0) )

blockSeq incrementSize2 startTime2 endTime2
|> Seq.iter (printfn "%A")



