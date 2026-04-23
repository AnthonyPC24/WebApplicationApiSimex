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
public async Task<ActionResult<Oferte>> PostOferte([FromBody] Oferte oferte)
{
    // Limpiar el ModelState para ignorar errores de navegación
    ModelState.Clear();

    oferte.Incoterm = null!;
    oferte.Operador = null!;
    oferte.TipusFluxe = null!;
    oferte.EstatOferta = null!;
    oferte.TipusCarrega = null!;
    oferte.TipusTransport = null!;
    oferte.TipusValidacio = null!;

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
                    var nuevo = new SeguimentOferte
                    {
                        OfertaId = id,
                        TrackingStepId = item.TrackingStepId,
                        EstatId = item.EstadoActualId ?? 1,
                        DataActualitzacio = DateTime.Now
                    };
                    // ← nulleamos las navegaciones igual que en PostOferte
                    nuevo.Estat = null!;
                    nuevo.Oferta = null!;
                    nuevo.TrackingStep = null!;
                    _context.SeguimentOfertes.Add(nuevo);
                }
            }

            // Si todos finalizados, marcar envío como Finalizado
            bool todosFinalizados = pasosActualizados.All(p => p.EstadoActualId == 3);
bool algunoAvanzado = pasosActualizados.Any(p => p.EstadoActualId == 2 || p.EstadoActualId == 3);

var envio = await _context.Envios.FirstOrDefaultAsync(e => e.OfertaId == id.ToString());
if (envio != null)
{
    if (todosFinalizados)
        envio.EstadoEnvio = "Entregado hoy";
    else if (algunoAvanzado)
        envio.EstadoEnvio = "En tránsito";
    else
        envio.EstadoEnvio = "En preparación";
    
            }

            await _context.SaveChangesAsync();
            result = Ok(new { mensaje = "Seguimiento actualizado" });
        }
    }
    catch (Exception ex)
    {
        result = StatusCode(500, new { mensaje = "Error al guardar", error = ex.Message, inner = ex.InnerException?.Message });
    }
    return result;
}

 [HttpPatch("{id}/estat")]
public async Task<IActionResult> UpdateEstatOferta(int id, [FromBody] UpdateEstatRequest request)
{
    var oferta = await _context.Ofertes.FindAsync(id);
    if (oferta == null) return NotFound();

    oferta.EstatOfertaId = request.EstatOfertaId;
    if (request.RaoRebuig != null) oferta.RaoRebuig = request.RaoRebuig;

    if (request.EstatOfertaId == 2)
    {
        var yaExiste = await _context.Envios
            .AnyAsync(e => e.OfertaId == id.ToString());

        if (!yaExiste)
        {
            var usuario = await _context.Usuaris.FindAsync(oferta.ClientId);
            var tipusTransport = await _context.TipusTransports.FindAsync(oferta.TipusTransportId);
            var portOrigen = oferta.PortOrigenId != null 
    ? await _context.Ports.FindAsync(oferta.PortOrigenId) : null;
var portDesti = oferta.PortDestiId != null 
    ? await _context.Ports.FindAsync(oferta.PortDestiId) : null;
var aeroOrigen = oferta.AeroportOrigenId != null 
    ? await _context.Aeroports.FindAsync(oferta.AeroportOrigenId) : null;
var aeroDesti = oferta.AeroportDestiId != null 
    ? await _context.Aeroports.FindAsync(oferta.AeroportDestiId) : null;

var origen = portOrigen?.Nom ?? aeroOrigen?.Nom ?? "";
var destino = portDesti?.Nom ?? aeroDesti?.Nom ?? "";

            var incoterm = await _context.Incoterms
         .Include(i => i.TipusInconterm)
            .FirstOrDefaultAsync(i => i.Id == oferta.IncotermId);
        var incotermCodi = incoterm?.TipusInconterm?.Codi ?? "FOB";

            var envio = new Envio
{
    OfertaId = id.ToString(),
    ClienteId = oferta.ClientId,
    EstadoEnvio = "En preparación",
    FechaPedido = DateOnly.FromDateTime(DateTime.Now),
    Cliente = usuario?.Nom ?? "Cliente",
    MetodoTransporte = tipusTransport?.Tipus ?? "Marítimo",
    Origen = origen,                              
    Destino = destino,                            
    ContenidoEnvio = oferta.Comentaris ?? "",     
    TipoDivisa = "EUR",
    Ruta = $"{origen} → {destino}",              
    PesoKg = oferta.PesBrut ?? 0,
    Incoterm = incotermCodi,
    Urgencia = "Media",
    Compania = usuario?.Empresa ?? ""
};
            _context.Envios.Add(envio);
        }
    }

    await _context.SaveChangesAsync();
    return Ok();
}
    }
}