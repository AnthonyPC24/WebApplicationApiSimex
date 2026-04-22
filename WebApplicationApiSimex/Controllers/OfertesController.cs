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
            var ofertas = await _context.Ofertes
                .Where(o => o.ClientId == clienteId)
                .OrderByDescending(o => o.Id)
                .ToListAsync();

            return ofertas;
        }

        // GET: api/Ofertes/Agente/7
        [HttpGet("Agente/{agenteId}")]
        public async Task<ActionResult<IEnumerable<Oferte>>> GetOfertesByAgente(int agentID)
        {
            return await _context.Ofertes
                .Where(o => o.AgentComercialId == agentID)
                .ToListAsync();        
        }
        //GET: /api/ofertes/{{id}}/Seguimiento
        //llama ofertas con sus incoterms y sus estados 
        [HttpGet("{id}/Seguimiento")]
        public async Task<ActionResult<SeguimientoOfertaDTO>> GetSeguimiento(int id)
        {
            SeguimientoOfertaDTO response = null;
            ActionResult result;

            try
            {
                // Cargamos Oferta -> Incoterm (intermedia) -> TipusInconterm (Nombre real)
                var oferta = await _context.Ofertes
                    .Include(o => o.Incoterm)
                        .ThenInclude(i => i.TipusInconterm)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (oferta == null)
                {
                    result = NotFound(new { mensaje = "Oferta no encontrada" });
                }
                else
                {
                    // Buscamos los pasos configurados para ese TIPO de incoterm
                    var pasosConfigurados = await _context.Incoterms
                        .Where(i => i.TipusIncontermId == oferta.IncotermId)
                        .Include(i => i.TrackingSteps)
                        .OrderBy(i => i.TrackingSteps.Ordre)
                        .ToListAsync();

                    var listaPasosDto = new List<PasoSeguimientoDTO>();

                    foreach (var item in pasosConfigurados)
                    {
                        var seguimientoReal = await _context.SeguimentOfertes
                            .FirstOrDefaultAsync(s => s.OfertaId == id && s.TrackingStepId == item.TrackingStepsId);

                        listaPasosDto.Add(new PasoSeguimientoDTO
                        {
                            TrackingStepId = item.TrackingStepsId,
                            NombrePaso = item.TrackingSteps.Nom,
                            Orden = item.TrackingSteps.Ordre ?? 0,
                            EstadoActualId = seguimientoReal?.EstatId ?? 1
                        });
                    }

                    response = new SeguimientoOfertaDTO
                    {
                        OfertaId = oferta.Id,
                        // Accedemos al nombre a través de TipusInconterm
                        IncotermNombre = oferta.Incoterm?.TipusInconterm?.Nom ?? "Sin Incoterm",
                        EstatEnvioGeneralId = oferta.EstatEnvioId,
                        Pasos = listaPasosDto
                    };

                    result = Ok(response);
                }
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new { mensaje = "Error interno", error = ex.Message });
            }

            return result;
        }

        [HttpPost("{id}/GuardarSeguimiento")]
        public async Task<IActionResult> GuardarSeguimiento(int id, [FromBody] List<PasoSeguimientoDTO> pasosActualizados)
        {
            IActionResult result;

            try
            {
                if (pasosActualizados == null || !pasosActualizados.Any())
                {
                    result = BadRequest(new { mensaje = "No se han recibido pasos" });
                }
                else if (!OferteExists(id))
                {
                    result = NotFound(new { mensaje = "La oferta no existe" });
                }
                else
                {
                    foreach (var item in pasosActualizados)
                    {
                        var seguimiento = await _context.SeguimentOfertes
                            .FirstOrDefaultAsync(s => s.OfertaId == id && s.TrackingStepId == item.TrackingStepId);

                        if (seguimiento != null)
                        {
                            seguimiento.EstatId = item.EstadoActualId ?? 1;
                            seguimiento.DataActualitzacio = DateTime.Now;
                        }
                        else
                        {
                            _context.SeguimentOfertes.Add(new SeguimentOferte
                            {
                                OfertaId = id,
                                TrackingStepId = item.TrackingStepId,
                                EstatId = item.EstadoActualId ?? 1,
                                DataActualitzacio = DateTime.Now
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                    result = Ok(new { mensaje = "Seguimiento actualizado" });
                }
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new { mensaje = "Error al guardar", error = ex.Message });
            }

            return result;
        }
    }
}
