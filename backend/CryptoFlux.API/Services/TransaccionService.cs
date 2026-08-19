using Microsoft.EntityFrameworkCore;
using CryptoFlux.API.Data;
using CryptoFlux.API.DTOs;
using CryptoFlux.API.Interfaces;
using CryptoFlux.API.Models;

namespace CryptoFlux.API.Services;

public class TransaccionService : ITransaccionService
{
    private readonly AppDbContext _context;
    private readonly ICryptoPriceService _priceService;

    public TransaccionService(AppDbContext context, ICryptoPriceService priceService)
    {
        _context = context;
        _priceService = priceService;
    }

    public async Task<IEnumerable<TransaccionResponseDto>> GetAllAsync()
    {
        return await _context.Transacciones
            .OrderByDescending(t => t.DateTime)
            .Select(t => new TransaccionResponseDto
            {
                Id = t.Id,
                CryptoCode = t.CryptoCode,
                Action = t.Action,
                CryptoAmount = t.CryptoAmount,
                Money = t.Money,
                DateTime = t.DateTime
            })
            .ToListAsync();
    }

    public async Task<TransaccionResponseDto?> GetByIdAsync(int id)
    {
        var t = await _context.Transacciones.FindAsync(id);
        if (t == null) return null;

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

    public async Task<TransaccionResponseDto> CreateAsync(TransaccionRequestDto dto)
    {
        decimal moneyCalculado = dto.Money;

        if (dto.Action.ToLower() == "compra")
        {
            var precio = await _priceService.GetPriceAsync(dto.CryptoCode, "ARS", dto.CryptoAmount);
            if (precio == null)
                throw new InvalidOperationException("No se pudo obtener la cotizacion de la criptomoneda.");

            moneyCalculado = dto.CryptoAmount * precio.Value;
        }

        if (dto.Action.ToLower() == "venta")
        {
            var saldo = await ObtenerSaldoAsync(dto.CryptoCode);
            if (dto.CryptoAmount > saldo)
                throw new InvalidOperationException(
                    $"Saldo insuficiente. Tenes {saldo} {dto.CryptoCode.ToUpper()} y queres vender {dto.CryptoAmount}.");

            var precio = await _priceService.GetPriceAsync(dto.CryptoCode, "ARS", dto.CryptoAmount);
            if (precio == null)
                throw new InvalidOperationException("No se pudo obtener la cotizacion de la criptomoneda.");

            moneyCalculado = dto.CryptoAmount * precio.Value;
        }

        var transaccion = new Transaccion
        {
            CryptoCode = dto.CryptoCode,
            Action = dto.Action,
            CryptoAmount = dto.CryptoAmount,
            Money = moneyCalculado,
            DateTime = dto.DateTime
        };

        _context.Transacciones.Add(transaccion);
        await _context.SaveChangesAsync();

        return new TransaccionResponseDto
        {
            Id = transaccion.Id,
            CryptoCode = transaccion.CryptoCode,
            Action = transaccion.Action,
            CryptoAmount = transaccion.CryptoAmount,
            Money = transaccion.Money,
            DateTime = transaccion.DateTime
        };
    }

    public async Task<bool> UpdateAsync(int id, TransaccionRequestDto dto)
    {
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

    private async Task<decimal> ObtenerSaldoAsync(string cryptoCode)
    {
        var transacciones = await _context.Transacciones
            .Where(t => t.CryptoCode.ToLower() == cryptoCode.ToLower())
            .ToListAsync();

        decimal saldo = 0;
        foreach (var t in transacciones)
        {
            if (t.Action.ToLower() == "compra")
                saldo += t.CryptoAmount;
            else if (t.Action.ToLower() == "venta")
                saldo -= t.CryptoAmount;
        }

        return saldo;
    }
}
