using Deducta.EcbExchangeRates.App.Dtos;

namespace Deducta.EcbExchangeRates.App.ExchangeRates;

public interface IExchangeRateRepository
{
    public Task<ExchangeRate> GetExchangeRatesFromRemote(CancellationToken cancellationToken = default);

    public Task<ExchangeRate> GetHistoricalRatesFromRemote(DateOnly dateOnly,
        CancellationToken cancellationToken = default);

    public Task<ExchangeRate> GetStoredExchangeRates(DateTimeOffset dateTimeOffset,
        CancellationToken cancellationToken = default);

    public Task<ExchangeRate> GetYearlyAverageExchangeRate(int year, CancellationToken cancellationToken = default);

    public Task StoreExchangeRates(List<ExchangeRate> exchangeRates, CancellationToken cancellationToken = default);
}