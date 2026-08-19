using System.Text.Json;
using System.Text.Json.Serialization;
using CryptoFlux.API.Interfaces;

namespace CryptoFlux.API.Services;

public class CryptoPriceService : ICryptoPriceService
{
    private readonly HttpClient _httpClient;

    public CryptoPriceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal?> GetPriceAsync(string coin, string fiat, decimal volume)
    {
        try
        {
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var volumeStr = volume.ToString("F8", culture);
            var url = $"https://criptoya.com/api/{coin.ToUpper()}/{fiat.ToUpper()}/{volumeStr}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var exchangePrices = JsonSerializer.Deserialize<Dictionary<string, ExchangeQuote>>(json);

            if (exchangePrices == null || exchangePrices.Count == 0)
                return null;

            var bestAsk = exchangePrices.Values
                .Where(e => e.Ask > 0)
                .MinBy(e => e.Ask);

            return bestAsk?.Ask;
        }
        catch
        {
            return null;
        }
    }
}

public class ExchangeQuote
{
    [JsonPropertyName("ask")]
    public decimal Ask { get; set; }

    [JsonPropertyName("totalAsk")]
    public decimal TotalAsk { get; set; }

    [JsonPropertyName("bid")]
    public decimal Bid { get; set; }

    [JsonPropertyName("totalBid")]
    public decimal TotalBid { get; set; }

    [JsonPropertyName("time")]
    public long Time { get; set; }
}
