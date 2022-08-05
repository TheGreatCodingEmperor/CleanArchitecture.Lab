using Microsoft.AspNetCore.Mvc;
using Api.Data;
using MassTransit;
using Masstransit.Test.Components.Contracts.Browse;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IRequestClient<BrowseCurrentData> _client;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IRequestClient<BrowseCurrentData> client
        )
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public IActionResult Test([FromBody]BrowseCurrentData query){
        var (success,failed) = await client.GetResponse<BrowseCurrentDataSuccess,BadRequestViewModel>(query);
        if(success.IsCompletedSuccessfully){
            return Results.Accepted(null,success.Result.Message);
        }
        if(failed.Result.Message.Status == 400){
            return Results.BadRequest($"{failed.Result.Message.Title}: {failed.Result.Message.Message}");
        }
        else{
            
        }
        return Results.BadRequest($"{failed.Result.Message.Title}: {failed.Result.Message.Message}");
    }
}
