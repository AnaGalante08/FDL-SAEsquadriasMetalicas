using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Data;
using SAEsquadrias.Api.DTOs;
using SAEsquadrias.Api.Models;

namespace SAEsquadrias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipe>>> GetEquipes()
        {
            return await _context.Equipes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Equipe>> GetEquipe(int id)
        {
            var equipe = await _context.Equipes.FindAsync(id);
            if (equipe == null) return NotFound();

            return equipe;
        }

        [HttpPost]
        public async Task<ActionResult<Equipe>> PostEquipe(EquipeDto dto)
        {
            var equipe = new Equipe
            {
                Nome = dto.Nome.Trim(),
                Responsavel = dto.Responsavel.Trim(),
                QuantidadeIntegrantes = dto.QuantidadeIntegrantes
            };

            _context.Equipes.Add(equipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEquipe), new { id = equipe.Id }, equipe);
        }
    }
}
