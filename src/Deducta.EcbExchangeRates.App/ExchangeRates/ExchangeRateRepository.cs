using System.Globalization;
using System.Xml.Linq;
using Deducta.EcbExchangeRates.App.Dtos;
using MongoDB.Driver;

namespace Deducta.EcbExchangeRates.App.ExchangeRates;

public class ExchangeRateRepository(HttpClient httpClient, string endpoint, IMongoCollection<ExchangeRate> collection)
    : IExchangeRateRepository
{
    public async Task<List<ExchangeRate>> GetExchangeRatesFromWeb(CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync(endpoint, cancellationToken);

        response.EnsureSuccessStatusCode();
        var xmlContent = await response.Content.ReadAsStringAsync(cancellationToken);

        return ParseExchangeRates(xmlContent);
    }

    public async Task<List<ExchangeRate>> GetStoredExchangeRates(DateTimeOffset date, CancellationToken cancellationToken = default)
    {
        var startOfDay = date.Date;
        var startOfNextDay = startOfDay.AddDays(1);
        
        var filter = Builders<ExchangeRate>.Filter.And(
            Builders<ExchangeRate>.Filter.Gte(x => x.Date, startOfDay.Ticks),
            Builders<ExchangeRate>.Filter.Lt(x => x.Date, startOfNextDay.Ticks)
        );
        var data = await collection.FindAsync(filter, cancellationToken: cancellationToken);
        return await data.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task StoreExchangeRates(List<ExchangeRate> exchangeRates, CancellationToken cancellationToken = default)
    {
        await collection.InsertManyAsync(exchangeRates, cancellationToken: cancellationToken);
    }

    private List<ExchangeRate> ParseExchangeRates(string xmlContent)
    {
        var today = DateTimeOffset.UtcNow;
        var exchangeRates = new List<ExchangeRate>();
        var xdoc = XDocument.Parse(xmlContent);

        XNamespace ns = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

        var cubes = xdoc
            .Descendants(ns + "Cube")
            .Elements(ns + "Cube");

        foreach (var cube in cubes)
        {
            var currency = cube.Attribute("currency")?.Value;
            var rate = cube.Attribute("rate")?.Value;

            if (!string.IsNullOrEmpty(currency) && !string.IsNullOrEmpty(rate))
            {
                exchangeRates.Add(new ExchangeRate
                {
                    Currency = currency,
                    Rate = decimal.Parse(rate,
                        CultureInfo.InvariantCulture),
                    Date = new DateTimeOffset(today.Year, today.Month, today.Day, 0, 0, 0, TimeSpan.Zero).Ticks
                });
            }
        }

        return exchangeRates;
    }
}