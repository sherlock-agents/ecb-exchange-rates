using Deducta.EcbExchangeRates.App.ExchangeRates;
using Microsoft.Azure.Functions.Worker;

namespace Deducta.EcbExchangeRates.App.Cron;

public class ReadEcbRates(IExchangeRateRepository exchangeRateRepository)
{
    [Function(nameof(ReadAndStoreExchangeRates))]
    public async Task ReadAndStoreExchangeRates([TimerTrigger("0 0 0 * * *")] TimerInfo timer, FunctionContext context)
    {
        var rates = await exchangeRateRepository.GetExchangeRatesFromRemote();
        await exchangeRateRepository.StoreExchangeRates([rates]);
    }
}