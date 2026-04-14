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
    public class TipusValidacionsController : ControllerBase
    {
        private readonly Simex09Context _context;

        public TipusValidacionsController(Simex09Context context)
        {
            _context = context;
        }

        // GET: api/TipusValidacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTipusValidacions()
        {
            return await _context.TipusValidacions.Select(v => new { id = v.Id, nom = v.Tipus }).ToListAsync();
        }

        // GET: api/TipusValidacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipusValidacion>> GetTipusValidacion(int id)
        {
            var tipusValidacion = await _context.TipusValidacions.FindAsync(id);

            if (tipusValidacion == null)
            {
                return NotFound();
            }

            return tipusValidacion;
        }

        // PUT: api/TipusValidacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipusValidacion(int id, TipusValidacion tipusValidacion)
        {
            if (id != tipusValidacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipusValidacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipusValidacionExists(id))
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

        // POST: api/TipusValidacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipusValidacion>> PostTipusValidacion(TipusValidacion tipusValidacion)
        {
            _context.TipusValidacions.Add(tipusValidacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipusValidacion", new { id = tipusValidacion.Id }, tipusValidacion);
        }

        // DELETE: api/TipusValidacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipusValidacion(int id)
        {
            var tipusValidacion = await _context.TipusValidacions.FindAsync(id);
            if (tipusValidacion == null)
            {
                return NotFound();
            }

            _context.TipusValidacions.Remove(tipusValidacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipusValidacionExists(int id)
        {
            return _context.TipusValidacions.Any(e => e.Id == id);
        }
    }
}
