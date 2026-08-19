using Microsoft.EntityFrameworkCore;
using CryptoFlux.API.Data;
using CryptoFlux.API.DTOs;
using CryptoFlux.API.Interfaces;

namespace CryptoFlux.API.Services;

public class CarteraService : ICarteraService
{
    private readonly AppDbContext _context;
    private readonly ICryptoPriceService _priceService;

    public CarteraService(AppDbContext context, ICryptoPriceService priceService)
    {
        _context = context;
        _priceService = priceService;
    }

    public async Task<CarteraResponseDto> ObtenerCarteraAsync()
    {
        var transacciones = await _context.Transacciones.ToListAsync();

        var holdings = new Dictionary<string, decimal>();

        foreach (var t in transacciones)
        {
            var code = t.CryptoCode.ToUpper();
            if (!holdings.ContainsKey(code))
                holdings[code] = 0;

            if (t.Action.ToLower() == "compra")
                holdings[code] += t.CryptoAmount;
            else if (t.Action.ToLower() == "venta")
                holdings[code] -= t.CryptoAmount;
        }

        var criptomonedas = new List<CarteraItemDto>();
        decimal totalCartera = 0;

        foreach (var holding in holdings)
        {
            if (holding.Value <= 0)
                continue;

            var precioUnitario = await _priceService.GetPriceAsync(holding.Key, "ARS", 1);
            var precio = precioUnitario ?? 0;
            var valor = holding.Value * precio;

            criptomonedas.Add(new CarteraItemDto
            {
                CryptoCode = holding.Key,
                CantidadActual = holding.Value,
                PrecioActual = precio,
                ValorEnDinero = valor
            });

            totalCartera += valor;
        }

        return new CarteraResponseDto
        {
            Criptomonedas = criptomonedas,
            TotalCartera = totalCartera
        };
    }
}
