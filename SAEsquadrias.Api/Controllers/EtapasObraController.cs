using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Data;
using SAEsquadrias.Api.Models;

namespace SAEsquadrias.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EtapasObraController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EtapasObraController(AppDbContext context)
        {
            _context = context;
        }

       
        [HttpGet("obra/{obraId}")]
        public async Task<IActionResult> ListarPorObra(int obraId)
        {
            var obra = await _context.Obras
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(o => o.Id == obraId && o.Ativo);

            if (obra == null)
                return NotFound("Obra não encontrada.");

            var etapas = await _context.EtapasObra
                .Where(e => e.ObraId == obraId)
                .OrderBy(e => e.Ordem)
                .Select(e => new
                {
                    e.Id,
                    e.Nome,
                    e.Ordem,
                    e.Status,
                    e.DataConclusao
                })
                .ToListAsync();

            int totalEtapas = etapas.Count;

            int etapasConcluidas = etapas.Count(e => e.Status == "Concluído");

            int percentual = 0;

            if (totalEtapas > 0)
            {
                percentual = etapasConcluidas * 100 / totalEtapas;
            }

            return Ok(new
            {
                ObraId = obra.Id,
                Cliente = obra.Cliente?.Nome,
                Endereco = $"{obra.Rua}, {obra.Numero} - {obra.Bairro}, {obra.Cidade} - {obra.Estado}",
                TotalEtapas = totalEtapas,
                EtapasConcluidas = etapasConcluidas,
                PercentualConcluido = percentual,
                Etapas = etapas
            });
        }

    
        [HttpPost]
        public async Task<IActionResult> Cadastrar(EtapaObra etapa)
        {
            var obraExiste = await _context.Obras
                .AnyAsync(o => o.Id == etapa.ObraId && o.Ativo);

            if (!obraExiste)
                return BadRequest("Obra não encontrada ou inativa.");

            etapa.Id = 0;

            _context.EtapasObra.Add(etapa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ListarPorObra),
                new { obraId = etapa.ObraId },
                etapa
            );
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, EtapaObra dados)
        {
            var etapa = await _context.EtapasObra.FindAsync(id);

            if (etapa == null)
                return NotFound("Etapa não encontrada.");

            etapa.Status = dados.Status;

            if (dados.Status == "Concluído")
                etapa.DataConclusao = DateTime.Now;
            else
                etapa.DataConclusao = null;

            await _context.SaveChangesAsync();

            return Ok(etapa);
        }
    }
}
