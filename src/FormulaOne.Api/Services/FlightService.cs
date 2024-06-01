using System.Text.Json;
using FormulaOne.Entities.DbSet;
using RestSharp;

namespace FormulaOne.Api.Services;

public class FlightService : IFlightService
{
    public async Task<List<FlightDto>> GetAllAvailableFlights()
    {
        const string url = "https://localhost:6877/api/FlightsCalendar";

        var client = new RestClient();
        var request = new RestRequest(url);

        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            throw new Exception("Something went wrong");
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
 
        return JsonSerializer.Deserialize<List<FlightDto>>(response.Content, options);
    }
}
