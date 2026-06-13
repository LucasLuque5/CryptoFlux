using System.ComponentModel.DataAnnotations;

namespace CryptoFlux.API.DTOs;

public class TransaccionRequestDto
{
    [Required]
    public string CryptoCode { get; set; } = string.Empty;

    [Required]
    public string Action { get; set; } = string.Empty;

    [Range(0.00000001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public decimal CryptoAmount { get; set; }

    public decimal Money { get; set; }

    public DateTime DateTime { get; set; }
}