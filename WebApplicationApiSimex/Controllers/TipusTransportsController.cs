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
    public class TipusTransportsController : ControllerBase
    {
        private readonly Simex09Context _context;

        public TipusTransportsController(Simex09Context context)
        {
            _context = context;
        }

        // GET: api/TipusTransports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipusTransport>>> GetTipusTransports()
        {
            var lista = await _context.TipusTransports
               .Select(i => new {
                   id = i.Id,
                   nom = i.Tipus
               })
               .ToListAsync();

            return Ok(lista);
        }

        // GET: api/TipusTransports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipusTransport>> GetTipusTransport(int id)
        {
            var tipusTransport = await _context.TipusTransports.FindAsync(id);

            if (tipusTransport == null)
            {
                return NotFound();
            }

            return tipusTransport;
        }

        // PUT: api/TipusTransports/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipusTransport(int id, TipusTransport tipusTransport)
        {
            if (id != tipusTransport.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipusTransport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipusTransportExists(id))
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

        // POST: api/TipusTransports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipusTransport>> PostTipusTransport(TipusTransport tipusTransport)
        {
            _context.TipusTransports.Add(tipusTransport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipusTransport", new { id = tipusTransport.Id }, tipusTransport);
        }

        // DELETE: api/TipusTransports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipusTransport(int id)
        {
            var tipusTransport = await _context.TipusTransports.FindAsync(id);
            if (tipusTransport == null)
            {
                return NotFound();
            }

            _context.TipusTransports.Remove(tipusTransport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipusTransportExists(int id)
        {
            return _context.TipusTransports.Any(e => e.Id == id);
        }
    }
}
