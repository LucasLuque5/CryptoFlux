using CryptoFlux.API.Interfaces;

namespace CryptoFlux.API.Services;

public class CryptoPriceService : ICryptoPriceService
{
    private readonly HttpClient _httpClient;

    public CryptoPriceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ExchangeQuote?> GetQuoteAsync(string cryptoCode)
    {
        try
        {
            var url = $"https://criptoya.com/api/satoshitango/{cryptoCode.ToLower()}/ars/1";
            return await _httpClient.GetFromJsonAsync<ExchangeQuote>(url);
        }
        catch
        {
            return null;
        }
    }
}

public class ExchangeQuote
{
    public decimal TotalAsk { get; set; }
    public decimal TotalBid { get; set; }
}
