using Microsoft.AspNetCore.Mvc;
using CryptoFlux.API.DTOs;
using CryptoFlux.API.Interfaces;

namespace CryptoFlux.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TransaccionesController : ControllerBase
{
    private readonly ITransaccionService _service;

    public TransaccionesController(ITransaccionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransaccionResponseDto>>> GetAll()
    {
        var transacciones = await _service.GetAllAsync();
        return Ok(transacciones);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransaccionResponseDto>> GetById(int id)
    {
        var transaccion = await _service.GetByIdAsync(id);
        if (transaccion == null) return NotFound();
        return Ok(transaccion);
    }

    [HttpPost]
    public async Task<ActionResult<TransaccionResponseDto>> Create([FromBody] TransaccionRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
             var transaccion = await _service.CreateAsync(dto);
             return CreatedAtAction(nameof(GetById), new { id = transaccion.Id }, transaccion);

        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TransaccionRequestDto dto)
    {
        var resultado = await _service.UpdateAsync(id, dto);
        if (!resultado) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _service.DeleteAsync(id);
        if (!resultado) return NotFound();
        return NoContent();
    }
}