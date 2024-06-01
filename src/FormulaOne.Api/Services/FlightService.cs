using System.Net;
using System.Text.Json;
using FormulaOne.Entities.DbSet;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RestSharp;

namespace FormulaOne.Api.Services;

public class FlightService : IFlightService
{
    private static readonly AsyncRetryPolicy<RestResponse> RetryPolicy = 
        Policy.HandleResult<RestResponse>(resp =>
                resp.StatusCode == HttpStatusCode.TooManyRequests || (int)resp.StatusCode >= 500)
            .WaitAndRetryAsync(2, retryAttempt =>
            {
                Console.WriteLine($"Attempt {retryAttempt} - Retrying due to error.");
                return TimeSpan.FromSeconds(5 + retryAttempt);
            });

    private static readonly AsyncCircuitBreakerPolicy<RestResponse> CircuitBreakerPolicy =
        Policy.HandleResult<RestResponse>(resp =>
                (int)resp.StatusCode >= 500)
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

    private static readonly AsyncCircuitBreakerPolicy<RestResponse> AdvanceCbPolicy =
        Policy.HandleResult<RestResponse>(resp =>
                (int)resp.StatusCode >= 500)
            .AdvancedCircuitBreakerAsync(
                0.5,
                TimeSpan.FromMinutes(1),
                10,
                TimeSpan.FromMinutes(1)
            );

    public async Task<List<FlightDto>> GetAllAvailableFlights()
    {
        if(CircuitBreakerPolicy.CircuitState == CircuitState.Open)
            throw new Exception("Service is not available");

        const string url = "https://localhost:6877/api/FlightsCalendar";

        var client = new RestClient();
        var request = new RestRequest(url);

        var response = await AdvanceCbPolicy.ExecuteAsync(
            async () => await RetryPolicy.ExecuteAsync(
                async () => await client.ExecuteAsync(request)
                )
            );

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
