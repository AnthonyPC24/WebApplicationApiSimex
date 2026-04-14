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
    public class AeroportsController : ControllerBase
    {
        private readonly Simex09Context _context;

        public AeroportsController(Simex09Context context)
        {
            _context = context;
        }

        // GET: api/Aeroports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aeroport>>> GetAeroports()
        {
            return await _context.Aeroports.ToListAsync();
        }

        // GET: api/Aeroports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aeroport>> GetAeroport(int id)
        {
            var aeroport = await _context.Aeroports.FindAsync(id);

            if (aeroport == null)
            {
                return NotFound();
            }

            return aeroport;
        }

        // PUT: api/Aeroports/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAeroport(int id, Aeroport aeroport)
        {
            if (id != aeroport.Id)
            {
                return BadRequest();
            }

            _context.Entry(aeroport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AeroportExists(id))
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

        // POST: api/Aeroports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Aeroport>> PostAeroport(Aeroport aeroport)
        {
            _context.Aeroports.Add(aeroport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAeroport", new { id = aeroport.Id }, aeroport);
        }

        // DELETE: api/Aeroports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAeroport(int id)
        {
            var aeroport = await _context.Aeroports.FindAsync(id);
            if (aeroport == null)
            {
                return NotFound();
            }

            _context.Aeroports.Remove(aeroport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AeroportExists(int id)
        {
            return _context.Aeroports.Any(e => e.Id == id);
        }
    }
}
