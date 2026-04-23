using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationApiSimex.Models;

namespace WebApplicationApiSimex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnviosController : ControllerBase
    {
        private readonly Simex09Context _context;

        public EnviosController(Simex09Context context)
        {
            _context = context;
        }

        // GET: api/Envios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Envio>>> GetEnvios()
        {
            return await _context.Envios.ToListAsync();
        }

        // GET: api/Envios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Envio>> GetEnvio(int id)
        {
            var envio = await _context.Envios.FindAsync(id);
            if (envio == null) return NotFound();
            return envio;
        }

        // GET: api/Envios/cliente/5
        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> GetEnviosByCliente(int clienteId)
        {
            var envios = await _context.Envios
                .Where(e => e.ClienteId == clienteId)
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            return Ok(envios);
        }
        [HttpGet("{id}/ofertaId")]
        public async Task<IActionResult> GetOfertaIdByEnvio(int id)
        {
       var envio = await _context.Envios.FindAsync(id);
       if (envio == null) return NotFound();
    
    
         var oferta = await _context.Ofertes
        .FirstOrDefaultAsync(o => o.Id.ToString() == envio.OfertaId);
    
     if (oferta == null) return NotFound();
     return Ok(new { ofertaId = oferta.Id });
}
// DELETE: api/Envios/5
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteEnvio(int id)
{
    var envio = await _context.Envios.FindAsync(id);
    if (envio == null) return NotFound();

  
    if (envio.OfertaId != null)
    {
        var ofertaId = int.Parse(envio.OfertaId);
        var seguimientos = _context.SeguimentOfertes
            .Where(s => s.OfertaId == ofertaId);
        _context.SeguimentOfertes.RemoveRange(seguimientos);

        // Borrar también la oferta
        var oferta = await _context.Ofertes.FindAsync(ofertaId);
        if (oferta != null) _context.Ofertes.Remove(oferta);
    }

    _context.Envios.Remove(envio);
    await _context.SaveChangesAsync();
    return NoContent();
}

    }
}