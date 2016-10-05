module HtmlTableTests

open NUnit.Framework
open FsUnit

let toStringReader t = new System.IO.StringReader(t)

[<Test>]
let ``parse table``() = 
    let sr = "<table id='myTable'><thead><th><th></td></tr></thead><tbody><tr><td>hello</td></tr></tbody></table>" |> toStringReader
    let table = HtmlTable.LoadHtml "myTable" sr
    
    table |> Seq.length |> should equal 1
    (table |> Seq.head).id |> should equal "myTable"


[<Test>]
let ``speed``()=
    Seq.initInfinite(fun i-> i |> int64)
    |> Seq.take 10000000
    |> Seq.map (fun i -> i.ToString() |> System.Int64.Parse )
    |> Seq.length
    |> (printfn "%A")