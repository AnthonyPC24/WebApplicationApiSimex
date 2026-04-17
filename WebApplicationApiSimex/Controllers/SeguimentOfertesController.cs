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
    public class SeguimentOfertesController : ControllerBase
    {
        private readonly Simex09Context _context;

        public SeguimentOfertesController(Simex09Context context)
        {
            _context = context;
        }

        // GET: api/SeguimentOfertes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeguimentOferte>>> GetSeguimentOfertes()
        {
            return await _context.SeguimentOfertes.ToListAsync();
        }

        // GET: api/SeguimentOfertes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeguimentOferte>> GetSeguimentOferte(int id)
        {
            var seguimentOferte = await _context.SeguimentOfertes.FindAsync(id);

            if (seguimentOferte == null)
            {
                return NotFound();
            }

            return seguimentOferte;
        }

        // PUT: api/SeguimentOfertes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeguimentOferte(int id, SeguimentOferte seguimentOferte)
        {
            if (id != seguimentOferte.Id)
            {
                return BadRequest();
            }

            _context.Entry(seguimentOferte).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeguimentOferteExists(id))
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

        // POST: api/SeguimentOfertes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SeguimentOferte>> PostSeguimentOferte(SeguimentOferte seguimentOferte)
        {
            _context.SeguimentOfertes.Add(seguimentOferte);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeguimentOferte", new { id = seguimentOferte.Id }, seguimentOferte);
        }

        // DELETE: api/SeguimentOfertes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeguimentOferte(int id)
        {
            var seguimentOferte = await _context.SeguimentOfertes.FindAsync(id);
            if (seguimentOferte == null)
            {
                return NotFound();
            }

            _context.SeguimentOfertes.Remove(seguimentOferte);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeguimentOferteExists(int id)
        {
            return _context.SeguimentOfertes.Any(e => e.Id == id);
        }
    }
}
