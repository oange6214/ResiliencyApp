using FormulaOne.Entities.DbSet;

namespace FormulaOne.AirlineService.Services;

public interface ICalendarService
{
    Task<List<FlightDto>> GetAvailableFlights();
}