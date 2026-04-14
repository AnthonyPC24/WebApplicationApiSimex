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
    public class IncotermsController : ControllerBase
    {
        private readonly Simex09Context _context;

        public IncotermsController(Simex09Context context)
        {
            _context = context;
        }

        // GET: api/Incoterms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetIncotermsList()
        {
            // Hacemos un Join para traer el ID de la tabla Incoterm 
            // pero el Nombre de la tabla TipusIncoterm
            return await _context.Incoterms
                .Select(i => new {
                    id = i.Id, // Este es el ID que guardaremos en la Oferta
                    nom = i.TipusInconterm.Codi // Ej: "CIF" o "FOB"
                })
                .ToListAsync();
        }

        // GET: api/Incoterms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Incoterm>> GetIncoterm(int id)
        {
            var incoterm = await _context.Incoterms.FindAsync(id);

            if (incoterm == null)
            {
                return NotFound();
            }

            return incoterm;
        }

        // PUT: api/Incoterms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncoterm(int id, Incoterm incoterm)
        {
            if (id != incoterm.Id)
            {
                return BadRequest();
            }

            _context.Entry(incoterm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncotermExists(id))
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

        // POST: api/Incoterms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Incoterm>> PostIncoterm(Incoterm incoterm)
        {
            _context.Incoterms.Add(incoterm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIncoterm", new { id = incoterm.Id }, incoterm);
        }

        // DELETE: api/Incoterms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncoterm(int id)
        {
            var incoterm = await _context.Incoterms.FindAsync(id);
            if (incoterm == null)
            {
                return NotFound();
            }

            _context.Incoterms.Remove(incoterm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IncotermExists(int id)
        {
            return _context.Incoterms.Any(e => e.Id == id);
        }
    }
}
