using Microsoft.AspNetCore.Mvc;
using CryptoFlux.API.DTOs;
using CryptoFlux.API.Interfaces;

namespace CryptoFlux.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CarteraController : ControllerBase
{
    private readonly ICarteraService _carteraService;

    public CarteraController(ICarteraService carteraService)
    {
        _carteraService = carteraService;
    }

    [HttpGet]
    public async Task<ActionResult<CarteraResponseDto>> ObtenerCartera()
    {
        try
        {
            var cartera = await _carteraService.ObtenerCarteraAsync();
            return Ok(cartera);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al calcular la cartera.", detail = ex.Message });
        }
    }
}
