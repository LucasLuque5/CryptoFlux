using Microsoft.EntityFrameworkCore;
using CryptoFlux.API.Data;
using CryptoFlux.API.DTOs;
using CryptoFlux.API.Interfaces;
using CryptoFlux.API.Models;

namespace CryptoFlux.API.Services;

public class TransaccionService : ITransaccionService
{
    private static readonly string[] CriptosValidas = { "btc", "eth", "usdc" };
    private static readonly string[] AccionesValidas = { "purchase", "sale" };

    private readonly AppDbContext _context;
    private readonly ICryptoPriceService _priceService;

    public TransaccionService(AppDbContext context, ICryptoPriceService priceService)
    {
        _context = context;
        _priceService = priceService;
    }

    public async Task<IEnumerable<TransaccionResponseDto>> GetAllAsync()
    {
        var transacciones = await _context.Transacciones
            .OrderByDescending(t => t.DateTime)
            .ToListAsync();

        return transacciones.Select(ToDto);
    }

    public async Task<TransaccionResponseDto?> GetByIdAsync(int id)
    {
        var transaccion = await _context.Transacciones.FindAsync(id);
        if (transaccion == null) return null;

        return ToDto(transaccion);
    }

    public async Task<TransaccionResponseDto> CreateAsync(TransaccionRequestDto dto)
    {
        Validar(dto);

        if (dto.Action == "sale")
        {
            var saldo = await ObtenerSaldoAsync(dto.CryptoCode);
            if (dto.CryptoAmount > saldo)
                throw new InvalidOperationException(
                    $"Saldo insuficiente. Tenes {saldo} {dto.CryptoCode.ToUpper()} y queres vender {dto.CryptoAmount}.");
        }

        var cotizacion = await _priceService.GetQuoteAsync(dto.CryptoCode);
        if (cotizacion == null)
            throw new InvalidOperationException("No se pudo obtener la cotizacion de la criptomoneda.");

        var precio = dto.Action == "purchase" ? cotizacion.TotalAsk : cotizacion.TotalBid;

        var transaccion = new Transaccion
        {
            CryptoCode = dto.CryptoCode,
            Action = dto.Action,
            CryptoAmount = dto.CryptoAmount,
            Money = dto.CryptoAmount * precio,
            DateTime = dto.DateTime
        };

        _context.Transacciones.Add(transaccion);
        await _context.SaveChangesAsync();

        return ToDto(transaccion);
    }

    public async Task<bool> UpdateAsync(int id, TransaccionRequestDto dto)
    {
        Validar(dto);

        var transaccion = await _context.Transacciones.FindAsync(id);
        if (transaccion == null) return false;

        transaccion.CryptoCode = dto.CryptoCode;
        transaccion.Action = dto.Action;
        transaccion.CryptoAmount = dto.CryptoAmount;
        transaccion.Money = dto.Money;
        transaccion.DateTime = dto.DateTime;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var transaccion = await _context.Transacciones.FindAsync(id);
        if (transaccion == null) return false;

        _context.Transacciones.Remove(transaccion);
        await _context.SaveChangesAsync();
        return true;
    }

    private static void Validar(TransaccionRequestDto dto)
    {
        if (!CriptosValidas.Contains(dto.CryptoCode))
            throw new InvalidOperationException("La criptomoneda no es valida.");

        if (!AccionesValidas.Contains(dto.Action))
            throw new InvalidOperationException("La accion debe ser purchase o sale.");

        if (dto.CryptoAmount <= 0)
            throw new InvalidOperationException("La cantidad debe ser mayor a 0.");

        if (dto.DateTime == default)
            throw new InvalidOperationException("La fecha es obligatoria.");
    }

    private async Task<decimal> ObtenerSaldoAsync(string cryptoCode)
    {
        var compras = await _context.Transacciones
            .Where(t => t.CryptoCode == cryptoCode && t.Action == "purchase")
            .SumAsync(t => t.CryptoAmount);

        var ventas = await _context.Transacciones
            .Where(t => t.CryptoCode == cryptoCode && t.Action == "sale")
            .SumAsync(t => t.CryptoAmount);

        return compras - ventas;
    }

    private static TransaccionResponseDto ToDto(Transaccion t)
    {
        return new TransaccionResponseDto
        {
            Id = t.Id,
            CryptoCode = t.CryptoCode,
            Action = t.Action,
            CryptoAmount = t.CryptoAmount,
            Money = t.Money,
            DateTime = t.DateTime
        };
    }
}
