using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationApiSimex.Models;

namespace WebApplicationApiSimex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertesController : ControllerBase
    {
        private readonly Simex09Context _context;

        public OfertesController(Simex09Context context)
        {
            _context = context;
        }

        // GET: api/Ofertes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Oferte>>> GetOfertes()
        {
            return await _context.Ofertes.ToListAsync();
        }

        // GET: api/Ofertes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Oferte>> GetOferte(int id)
        {
            var oferte = await _context.Ofertes.FindAsync(id);

            if (oferte == null)
            {
                return NotFound();
            }

            return oferte;
        }

        // PUT: api/Ofertes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOferte(int id, Oferte oferte)
        {
            if (id != oferte.Id)
            {
                return BadRequest();
            }

            _context.Entry(oferte).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OferteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ofertes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Oferte>> PostOferte(Oferte oferte)
        {
            _context.Ofertes.Add(oferte);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOferte", new { id = oferte.Id }, oferte);
        }

        // DELETE: api/Ofertes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOferte(int id)
        {
            var oferte = await _context.Ofertes.FindAsync(id);
            if (oferte == null)
            {
                return NotFound();
            }

            _context.Ofertes.Remove(oferte);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OferteExists(int id)
        {
            return _context.Ofertes.Any(e => e.Id == id);
        }

        // GET: api/Ofertes/Cliente/5
        [HttpGet("Cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Oferte>>> GetOfertesByCliente(int clienteId)
        {
            return await _context.Ofertes
                .Where(o=> o.ClientId == clienteId)
                .OrderByDescending(o=> o.Id)
                .ToListAsync();
        }

        // GET: api/Ofertes/Agente/7
        [HttpGet("Agente/{agenteId}")]
        public async Task<ActionResult<IEnumerable<Oferte>>> GetOfertesByAgente(int agentID)
        {
            return await _context.Ofertes
                .Where(o => o.AgentComercialId == agentID)
                .ToListAsync();        }
    }
}
