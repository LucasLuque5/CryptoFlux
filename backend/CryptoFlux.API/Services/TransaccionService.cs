using Microsoft.EntityFrameworkCore;
using CryptoFlux.API.Data;
using CryptoFlux.API.DTOs;
using CryptoFlux.API.Interfaces;
using CryptoFlux.API.Models;

namespace CryptoFlux.API.Services;

public class TransaccionService : ITransaccionService
{
    private readonly AppDbContext _context;

    public TransaccionService(AppDbContext context)
    {
        _context = context;
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

        var transaccion = new Transaccion
        {
            CryptoCode = dto.CryptoCode,
            Action = dto.Action,
            CryptoAmount = dto.CryptoAmount,
            Money = dto.Money,
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

}