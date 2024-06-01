using FormulaOne.Entities.DbSet;

namespace FormulaOne.Api.Services;

public interface IFlightService
{
    Task<List<FlightDto>> GetAllAvailableFlights();
}
