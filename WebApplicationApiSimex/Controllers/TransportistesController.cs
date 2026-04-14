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
    public class TransportistesController : ControllerBase
    {
        private readonly Simex09Context _context;

        public TransportistesController(Simex09Context context)
        {
            _context = context;
        }

        // GET: api/Transportistes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTransportistes()
        {
            return await _context.Transportistes.Select(t => new { id = t.Id, nom = t.Nom }).ToListAsync();
        }

        // GET: api/Transportistes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transportiste>> GetTransportiste(int id)
        {
            var transportiste = await _context.Transportistes.FindAsync(id);

            if (transportiste == null)
            {
                return NotFound();
            }

            return transportiste;
        }

        // PUT: api/Transportistes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransportiste(int id, Transportiste transportiste)
        {
            if (id != transportiste.Id)
            {
                return BadRequest();
            }

            _context.Entry(transportiste).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportisteExists(id))
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

        // POST: api/Transportistes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transportiste>> PostTransportiste(Transportiste transportiste)
        {
            _context.Transportistes.Add(transportiste);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransportiste", new { id = transportiste.Id }, transportiste);
        }

        // DELETE: api/Transportistes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransportiste(int id)
        {
            var transportiste = await _context.Transportistes.FindAsync(id);
            if (transportiste == null)
            {
                return NotFound();
            }

            _context.Transportistes.Remove(transportiste);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransportisteExists(int id)
        {
            return _context.Transportistes.Any(e => e.Id == id);
        }
    }
}
