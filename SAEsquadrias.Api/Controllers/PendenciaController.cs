using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Data;
using SAEsquadrias.Api.DTOs;
using SAEsquadrias.Api.Models;

namespace SAEsquadrias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PendenciaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PendenciaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PendenciaDto>>> GetPendencias([FromQuery] string? busca)
        {
            var query = _context.Pendencias
                .Include(p => p.Obra)

                .Include(p => p.Vendedor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                busca = busca.ToLower();
                query = query.Where(p =>
                    p.ObraId.ToString().Contains(busca) || 
                    p.Descricao.ToLower().Contains(busca)
       
                );
            }

            var pendencias = await query.Select(p => new PendenciaDto
            {
                Id = p.Id,
                CodigoProjeto = $"#{p.ObraId:D4}", 
                Descricao = p.Descricao,
                ResponsavelNome = p.Vendedor != null ? p.Vendedor.Nome : "Vendedor",
                Status = p.Status
            }).ToListAsync();

            return Ok(pendencias);
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Pendencia>> GetPendencia(int id)
        {
            var pendencia = await _context.Pendencias
                .Include(p => p.Obra)
                .Include(p => p.Vendedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pendencia == null)
                return NotFound("Pendência não encontrada.");

            return Ok(pendencia);
        }

        [HttpPost]
        public async Task<ActionResult<Pendencia>> PostPendencia(PendenciaRequestDto dto)
        {
            var pendencia = new Pendencia
            {
                Descricao = dto.Descricao,
                Status = dto.Status,
                ObraId = dto.ObraId,
                VendedorId = dto.VendedorId
            };

            _context.Pendencias.Add(pendencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPendencia), new { id = pendencia.Id }, pendencia);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] string novoStatus)
        {
            var pendencia = await _context.Pendencias.FindAsync(id);
            if (pendencia == null) return NotFound();

            pendencia.Status = novoStatus;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}