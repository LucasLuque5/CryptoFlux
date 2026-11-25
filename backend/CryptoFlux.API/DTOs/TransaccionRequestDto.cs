namespace CryptoFlux.API.DTOs;

public class TransaccionRequestDto
{
    public string CryptoCode { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public decimal CryptoAmount { get; set; }
    public decimal Money { get; set; }
    public DateTime DateTime { get; set; }
}
