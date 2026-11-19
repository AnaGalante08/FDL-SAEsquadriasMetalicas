using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Data;
using SAEsquadrias.Api.Dtos;
using SAEsquadrias.Api.DTOs;
using SAEsquadrias.Api.Models;

namespace SAEsquadrias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdministradoresController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/administradores
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] AdministradorDto req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = req.Email.Trim().ToLower();

    
            var emailExiste = await _context.Administradores
                .AnyAsync(a => a.Email.ToLower() == email);

            if (emailExiste)
            {
                return Conflict(new { erro = "Já existe um administrador com este e-mail." });
            }


            var administrador = new Administrador
            {
                Nome = req.Nome.Trim(),
                Email = email,
                Telefone = string.IsNullOrWhiteSpace(req.Telefone) ? null : req.Telefone.Trim()
            };

            _context.Administradores.Add(administrador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = administrador.Id }, administrador);
        }

     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Administrador>>> Listar()
        {
            var administradores = await _context.Administradores
                .Where(a => a.Ativo) 
                .ToListAsync();

            return Ok(administradores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Administrador>> ObterPorId(int id)
        {
            var administrador = await _context.Administradores.FindAsync(id);

            if (administrador == null || !administrador.Ativo)
            {
                return NotFound(new { erro = "Administrador não encontrado." });
            }

            return administrador;
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Desativar(int id)
        {
            var administrador = await _context.Administradores.FindAsync(id);

            if (administrador == null)
            {
                return NotFound(new { erro = "Administrador não encontrado." });
            }

            administrador.Ativo = false;
            _context.Administradores.Update(administrador);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}