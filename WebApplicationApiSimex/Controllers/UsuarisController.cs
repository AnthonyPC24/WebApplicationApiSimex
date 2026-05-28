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
    public class UsuarisController : ControllerBase
    {
        private readonly Simex09Context _context;

        public UsuarisController(Simex09Context context)
        {
            _context = context;
        }

        // GET: api/Usuaris
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuari>>> GetUsuaris()
        {
            return await _context.Usuaris.ToListAsync();
        }

        // GET: api/Usuaris/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuari>> GetUsuari(int id)
        {
            var usuari = await _context.Usuaris.FindAsync(id);

            if (usuari == null)
            {
                return NotFound();
            }

            return usuari;
        }

        // PUT: api/Usuaris/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuari(int id, Usuari usuari)
        {
            if (id != usuari.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuari).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariExists(id))
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

        // POST: api/Usuaris
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuari>> PostUsuari(Usuari usuari)
        {
            _context.Usuaris.Add(usuari);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuari", new { id = usuari.Id }, usuari);
        }

        // DELETE: api/Usuaris/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuari(int id)
        {
            var usuari = await _context.Usuaris.FindAsync(id);
            if (usuari == null)
            {
                return NotFound();
            }

            _context.Usuaris.Remove(usuari);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuariExists(int id)
        {
            return _context.Usuaris.Any(e => e.Id == id);
        }

        // GET: api/Usuaris/resumen
        [HttpGet("resumen")]
        public async Task<IActionResult> GetResumenUsuaris()
        {
            var resumen = await _context.Usuaris
                .Include(u => u.Rol)  
                .Select(u => new
                {
                    Nom = u.Nom,
                    Cognoms = u.Cognoms,
                    Empresa = u.Empresa,
                    Rol = u.Rol.Rol1   
                })
                .ToListAsync();

            return Ok(resumen);
        }

        // Solo muestra clientes
        // GET: api/Usuaris/rol/3
        [HttpGet("rol/{idRol}")]
        public async Task<IActionResult> GetUsuariosPorRol(int idRol)
        {
            var usuarios = await _context.Usuaris
                .Include(u => u.Rol)
                .Where(u => u.RolId == idRol) 
                .Select(u => new
                {
                    Id = u.Id,
                    Nom = u.Nom,
                    Cognoms = u.Cognoms,
                    Empresa = u.Empresa,
                    Rol = u.Rol.Rol1
                })
                .ToListAsync();

            return Ok(usuarios);
        }
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest login)
{
    var user = await _context.Usuaris
        .FirstOrDefaultAsync(u => u.Correu == login.Usuario 
                              && u.Contrasenya == login.Password);

    if (user == null)
    {
        return Unauthorized(); // 401
    }

    return Ok(user); // 200
}

[HttpGet("contadors/{clientId}")]
public async Task<IActionResult> GetContadors(int clientId)
{
    var contadors = await _context.Envios
        .Where(e => e.ClienteId == clientId && e.EstadoEnvio != null)  // ← filtra nulls
        .GroupBy(e => e.EstadoEnvio)
        .Select(g => new { Estat = g.Key, Count = g.Count() })
        .ToListAsync();

    return Ok(contadors);
}

[HttpPatch("perfil/{id}")]
public async Task<IActionResult> UpdatePerfil(int id, [FromBody] UpdatePerfilRequest request)
{
    var usuari = await _context.Usuaris.FindAsync(id);
    if (usuari == null) return NotFound();

    if (request.Nom != null) usuari.Nom = request.Nom;
    if (request.Empresa != null) usuari.Empresa = request.Empresa;
    if (request.Telefon != null) usuari.Telefon = request.Telefon;

    await _context.SaveChangesAsync();
    return Ok(usuari);
}

// Subir DNI
[HttpPatch("dni/{id}")]
public async Task<IActionResult> UpdateDni(int id, [FromBody] UpdatePerfilRequest request)
{
    var usuari = await _context.Usuaris.FindAsync(id);
    if (usuari == null) return NotFound();

    usuari.DniFoto = request.DniFoto;
    await _context.SaveChangesAsync();
    return Ok();
}

// Descargar DNI m
[HttpGet("dni/{id}")]
public async Task<IActionResult> GetDni(int id)
{
    var usuari = await _context.Usuaris.FindAsync(id);
    if (usuari == null || usuari.DniFoto == null) return NotFound();
    return Ok(new { dniFoto = usuari.DniFoto });
}

// DylanExtra
[HttpGet("nombres")]
public async Task<IActionResult> GetNombres()
{
    var nombres = await _context.Usuaris
        .Select(u => u.Nom) //coge los nombres y los devuelve en forma de listas
        .ToListAsync();
    return Ok(nombres);
}


[HttpPost("partida")]
public async Task<IActionResult> PostPartida([FromBody] PartidaRequest request)
{
    var partida = new Partida
    {
        UsuariId   = request.UsuariId,
        Puntuacion = request.Puntuacion,
        Data       = DateTime.Now
    };

    _context.Partides.Add(partida);
    await _context.SaveChangesAsync();
    return Ok();
}


[HttpPut("record/{id}")]
public async Task<IActionResult> UpdateRecord(int id, [FromBody] RecordRequest request)
{
    var usuari = await _context.Usuaris.FindAsync(id); //busca a usuario en la base de datos por su id
    if (usuari == null) return NotFound();

    usuari.Puntuacion = request.Puntuacion;
    await _context.SaveChangesAsync();
    return Ok();
}


[HttpGet("record/{id}")]
public async Task<IActionResult> GetRecord(int id)
{
    var usuari = await _context.Usuaris.FindAsync(id);
    if (usuari == null) return NotFound();
    return Ok(usuari.Puntuacion ?? 0); //Puede ser que un usuario aun no tenga puntuacion y sea null asi que si es el caso pongo el 0 al lado de los ??
}
    

[HttpDelete("record/{id}")]
public async Task<IActionResult> DeleteRecord(int id)
{
    var usuari = await _context.Usuaris.FindAsync(id);
    if (usuari == null) return NotFound();

    usuari.Puntuacion = 0;
    await _context.SaveChangesAsync();
    return Ok();
}
}
}
