using CryptoFlux.API.DTOs;

namespace CryptoFlux.API.Interfaces;

public interface ICarteraService
{
    Task<CarteraResponseDto> ObtenerCarteraAsync();
}
