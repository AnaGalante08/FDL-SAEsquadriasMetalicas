using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Data;
using SAEsquadrias.Api.Models;
using SAEsquadrias.Api.Dtos;

namespace SAEsquadrias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VendedoresController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/vendedores
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] VendedorDto req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = req.Email.Trim().ToLower();

            // Verifica se o e-mail já existe
            var emailExiste = await _context.Vendedores
                .AnyAsync(v => v.Email.ToLower() == email);

            if (emailExiste)
            {
                return Conflict(new { erro = "Já existe um vendedor com este e-mail." });
            }

            // Mapeia os dados do DTO para o Model
            var vendedor = new Vendedor
            {
                Nome = req.Nome.Trim(),
                Email = email,
                Telefone = string.IsNullOrWhiteSpace(req.Telefone) ? null : req.Telefone.Trim()
            };

            _context.Vendedores.Add(vendedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = vendedor.Id }, vendedor);
        }

        // GET: api/vendedores (Usado para popular o select no front-end)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendedor>>> Listar()
        {
            var vendedores = await _context.Vendedores
                .Where(v => v.Ativo) // Traz apenas os ativos
                .ToListAsync();

            return Ok(vendedores);
        }

        // GET: api/vendedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendedor>> ObterPorId(int id)
        {
            var vendedor = await _context.Vendedores.FindAsync(id);

            if (vendedor == null || !vendedor.Ativo)
            {
                return NotFound(new { erro = "Vendedor não encontrado." });
            }

            return vendedor;
        }

        // DELETE: api/vendedores/5 (Exclusão Lógica)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Desativar(int id)
        {
            var vendedor = await _context.Vendedores.FindAsync(id);

            if (vendedor == null)
            {
                return NotFound(new { erro = "Vendedor não encontrado." });
            }

            vendedor.Ativo = false;
            _context.Vendedores.Update(vendedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}