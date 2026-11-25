using CryptoFlux.API.Services;

namespace CryptoFlux.API.Interfaces;

public interface ICryptoPriceService
{
    Task<ExchangeQuote?> GetQuoteAsync(string cryptoCode);
}
