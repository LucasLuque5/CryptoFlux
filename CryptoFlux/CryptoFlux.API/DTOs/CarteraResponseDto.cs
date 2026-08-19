namespace CryptoFlux.API.DTOs;

public class CarteraItemDto
{
    public string CryptoCode { get; set; } = string.Empty;
    public decimal CantidadActual { get; set; }
    public decimal PrecioActual { get; set; }
    public decimal ValorEnDinero { get; set; }
}

public class CarteraResponseDto
{
    public List<CarteraItemDto> Criptomonedas { get; set; } = new();
    public decimal TotalCartera { get; set; }
}
