namespace CryptoFlux.API.Interfaces;

public interface ICryptoPriceService
{
    Task<decimal?> GetPriceAsync(string coin, string fiat, decimal volume);
}
