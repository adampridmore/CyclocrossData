namespace FsCxDataWeb.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open FsCxDataWeb

open FSharp.Data
open FSharp.Charting
open System
open HtmlParse
open Types
open LapRender
[<ApiController>]

[<Route("[controller]")>]
type WeatherForecastController (logger : ILogger<WeatherForecastController>) =
    inherit ControllerBase()

    let summaries =
        [|
            "Freezing"
            "Bracing"
            "Chilly"
            "Cool"
            "Mild"
            "Warm"
            "Balmy"
            "Hot"
            "Sweltering"
            "Scorching"
        |]

    [<HttpGet>]
    member _.Get() =
        let rng = System.Random()
        [|
            for index in 0..4 ->
                { Date = DateTime.Now.AddDays(float index)
                  TemperatureC = rng.Next(-20,55)
                  Summary = summaries.[rng.Next(summaries.Length)] }
        |]

    [<HttpGet("cheese")>]
    member _.Get2() =

        // Winter 2021
        let url = @"http://results.smartiming.co.uk/view-race/yycard3mv40senior/"  // RD3

        let chart : XPlot.GoogleCharts.GoogleChart = 
            url 
            |> riderAndLapsFromHtml 
            |> toGoogleChart

        let content = chart.GetHtml()

        Microsoft.AspNetCore.Mvc.ContentResult(
            ContentType = "text/html",
            StatusCode = (int) System.Net.HttpStatusCode.OK,
            Content = content //"<html><body>Welcome2</body></html>"
        )
