namespace FormulaOne.Entities.DbSet;

public class FlightDto
{
    public string Arrival { get; set; } = string.Empty;
    public string Departure { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime FlightDate { get; set; }
}