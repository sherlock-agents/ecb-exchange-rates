using Deducta.EcbExchangeRates.App.Dtos;

namespace Deducta.EcbExchangeRates.App.ExchangeRates;

public interface IExchangeRateRepository
{
    public Task<List<ExchangeRate>> GetExchangeRatesFromWeb(CancellationToken cancellationToken = default);

    public Task<List<ExchangeRate>> GetStoredExchangeRates(DateTimeOffset dateTimeOffset, CancellationToken cancellationToken = default);

    public Task StoreExchangeRates(List<ExchangeRate> exchangeRates, CancellationToken cancellationToken = default);
}