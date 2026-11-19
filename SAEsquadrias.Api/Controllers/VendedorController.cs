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

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] VendedorDto req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = req.Email.Trim().ToLower();

          
            var emailExiste = await _context.Vendedores
                .AnyAsync(v => v.Email.ToLower() == email);

            if (emailExiste)
            {
                return Conflict(new { erro = "Já existe um vendedor com este e-mail." });
            }

           
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

     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendedor>>> Listar()
        {
            var vendedores = await _context.Vendedores
                .Where(v => v.Ativo) 
                .ToListAsync();

            return Ok(vendedores);
        }

      
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