using FormulaOne.Entities.DbSet;

namespace FormulaOne.AirlineService.Services;

public class CalendarService : ICalendarService
{
    private DateTime _recoveryTime = DateTime.UtcNow;
    private static readonly Random Random = new();

    public Task<List<FlightDto>> GetAvailableFlights()
    {
        
        if (_recoveryTime > DateTime.UtcNow)
        {
            throw new Exception("Service not available");
        }

        if (_recoveryTime < DateTime.UtcNow && Random.Next(1, 4) == 1)
        {
            _recoveryTime = DateTime.UtcNow.AddSeconds(25);
        }

        var flights = new List<FlightDto>
        {
            new ()
            {
                Arrival = "London",
                Departure = "Dubai",
                Price = 10000,
                FlightDate = DateTime.Now.AddDays(3)
            },
            new ()
            {
                Arrival = "Monaco",
                Departure = "Miami",
                Price = 14000,
                FlightDate = DateTime.Now.AddDays(3)
            },
            new ()
            {
                Arrival = "Madrid",
                Departure = "Singapore",
                Price = 9000,
                FlightDate = DateTime.Now.AddDays(3)
            },
            new ()
            {
                Arrival = "Athens",
                Departure = "Tokyo",
                Price = 18000,
                FlightDate = DateTime.Now.AddDays(3)
            },
            new ()
            {
                Arrival = "New York",
                Departure = "Porto",
                Price = 6000,
                FlightDate = DateTime.Now.AddDays(2)
            }
        };

        return Task.FromResult(flights);
    }
}
