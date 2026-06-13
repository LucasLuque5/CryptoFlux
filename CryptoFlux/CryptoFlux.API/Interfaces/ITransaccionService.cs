using CryptoFlux.API.DTOs;

namespace CryptoFlux.API.Interfaces;

public interface ITransaccionService
{
    Task<IEnumerable<TransaccionResponseDto>> GetAllAsync();
    Task<TransaccionResponseDto?> GetByIdAsync(int id);
    Task<TransaccionResponseDto> CreateAsync(TransaccionRequestDto dto);
    Task<bool> UpdateAsync(int id, TransaccionRequestDto dto);
    Task<bool> DeleteAsync(int id);
}
