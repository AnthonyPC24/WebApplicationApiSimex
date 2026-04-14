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
    public class TipusIncotermsController : ControllerBase
    {
        private readonly Simex09Context _context;

        public TipusIncotermsController(Simex09Context context)
        {
            _context = context;
        }

        // GET: api/TipusIncoterms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipusIncoterm>>> GetTipusIncoterms()
        {
            
            var lista = await _context.TipusIncoterms
                .Select(i => new {
                    id = i.Id,
                    nom = i.Codi
                })
                .ToListAsync();

            return Ok(lista);
        }

        // GET: api/TipusIncoterms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipusIncoterm>> GetTipusIncoterm(int id)
        {
            var tipusIncoterm = await _context.TipusIncoterms.FindAsync(id);

            if (tipusIncoterm == null)
            {
                return NotFound();
            }

            return tipusIncoterm;
        }

        // PUT: api/TipusIncoterms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipusIncoterm(int id, TipusIncoterm tipusIncoterm)
        {
            if (id != tipusIncoterm.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipusIncoterm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipusIncotermExists(id))
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

        // POST: api/TipusIncoterms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipusIncoterm>> PostTipusIncoterm(TipusIncoterm tipusIncoterm)
        {
            _context.TipusIncoterms.Add(tipusIncoterm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipusIncoterm", new { id = tipusIncoterm.Id }, tipusIncoterm);
        }

        // DELETE: api/TipusIncoterms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipusIncoterm(int id)
        {
            var tipusIncoterm = await _context.TipusIncoterms.FindAsync(id);
            if (tipusIncoterm == null)
            {
                return NotFound();
            }

            _context.TipusIncoterms.Remove(tipusIncoterm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipusIncotermExists(int id)
        {
            return _context.TipusIncoterms.Any(e => e.Id == id);
        }
    }
}
