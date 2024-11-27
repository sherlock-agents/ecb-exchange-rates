namespace Deducta.EcbExchangeRates.App.Dtos;

public class RateDto
{
    public required decimal Rate { get; set; }
    
    public required string CurrencyCode { get; set; }
}