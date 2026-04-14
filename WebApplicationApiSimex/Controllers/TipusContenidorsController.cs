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
    public class TipusContenidorsController : ControllerBase
    {
        private readonly Simex09Context _context;

        public TipusContenidorsController(Simex09Context context)
        {
            _context = context;
        }

        // GET: api/TipusContenidors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTipusContenidors()
        {
            return await _context.TipusContenidors.Select(c => new { id = c.Id, nom = c.Tipus }).ToListAsync();
        }

        // GET: api/TipusContenidors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipusContenidor>> GetTipusContenidor(int id)
        {
            var tipusContenidor = await _context.TipusContenidors.FindAsync(id);

            if (tipusContenidor == null)
            {
                return NotFound();
            }

            return tipusContenidor;
        }

        // PUT: api/TipusContenidors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipusContenidor(int id, TipusContenidor tipusContenidor)
        {
            if (id != tipusContenidor.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipusContenidor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipusContenidorExists(id))
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

        // POST: api/TipusContenidors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipusContenidor>> PostTipusContenidor(TipusContenidor tipusContenidor)
        {
            _context.TipusContenidors.Add(tipusContenidor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipusContenidor", new { id = tipusContenidor.Id }, tipusContenidor);
        }

        // DELETE: api/TipusContenidors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipusContenidor(int id)
        {
            var tipusContenidor = await _context.TipusContenidors.FindAsync(id);
            if (tipusContenidor == null)
            {
                return NotFound();
            }

            _context.TipusContenidors.Remove(tipusContenidor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipusContenidorExists(int id)
        {
            return _context.TipusContenidors.Any(e => e.Id == id);
        }
    }
}
