namespace Deducta.EcbExchangeRates.App.Dtos;

public class ExchangeRate
{
    public required string Currency { get; set; }

    public required decimal Rate { get; set; }
    
    public required long Date { get; set; }
}