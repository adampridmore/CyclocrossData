module HtmlTableTests

open NUnit.Framework
open FsUnit

let toStringReader t = new System.IO.StringReader(t)

[<Test>]
let ``parse table``() = 
    let sr = "<table id='myTable'><thead><tr><th>column1</th></tr></thead><tbody><tr><td>hello</td></tr></tbody></table>" |> toStringReader
    let tables = HtmlTable.LoadHtml "myTable" sr
    
    tables |> Seq.length |> should equal 1

    let table = tables |> Seq.head
    table.id |> should equal "myTable"
    table.columns |> Seq.length |> should equal 1
    table.columns.[0] |> should equal "column1"

    table.rows|> Seq.length |> should equal 1
    table.rows.[0].values  |> Seq.length |> should equal 1
    table.rows.[0].values.[0] |> should equal "hello"

[<Test>]
let ``parse table with th for row data``() = 
    let sr = "<table id='myTable'><thead><tr><th>column1</th></tr></thead><tbody><tr><th>hello</th></tr></tbody></table>" |> toStringReader
    let tables = HtmlTable.LoadHtml "myTable" sr
    
    tables |> Seq.length |> should equal 1

    let table = tables |> Seq.head
    table.id |> should equal "myTable"
    table.columns |> Seq.length |> should equal 1
    table.columns.[0] |> should equal "column1"

    table.rows|> Seq.length |> should equal 1
    table.rows.[0].values  |> Seq.length |> should equal 1
    table.rows.[0].values.[0] |> should equal "hello"

