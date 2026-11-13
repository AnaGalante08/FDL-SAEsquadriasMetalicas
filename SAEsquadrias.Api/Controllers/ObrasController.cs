using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadrias.API.Data;
using SAEsquadrias.API.DTOs;
using SAEsquadrias.API.Models;

namespace SAEsquadrias.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObrasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ObrasController(AppDbContext context)
        {
            _context = context;
        }

     
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var obra = await _context.Obras
                .Include(o => o.Etapas)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obra == null)
            {
                return NotFound();
            }

            var resposta = new ObraRespostaDTO
            {
                Id = obra.Id,
                NomeObra = obra.NomeObra,
                PercentualConcluido = obra.PercentualConcluido,
                Etapas = obra.Etapas.Select(e => new EtapaObraRespostaDTO
                {
                    Id = e.Id,
                    NomeEtapa = e.NomeEtapa,
                    Status = e.Status
                }).ToList()
            };

            return Ok(resposta);
        }

     
        [HttpPost]
        public async Task<IActionResult> Post(ObraCadastroDTO dados)
        {
            var obra = new Obra
            {
                NomeObra = dados.NomeObra,
                PercentualConcluido = dados.PercentualConcluido,
                Etapas = dados.Etapas.Select(e => new EtapaObra
                {
                    NomeEtapa = e.NomeEtapa,
                    Status = e.Status
                }).ToList()
            };

            _context.Obras.Add(obra);

            await _context.SaveChangesAsync();

            return Ok("Obra cadastrada com sucesso!");
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ObraAtualizacaoDTO dados)
        {
            var obra = await _context.Obras
                .Include(o => o.Etapas)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obra == null)
            {
                return NotFound();
            }

            obra.NomeObra = dados.NomeObra;
            obra.PercentualConcluido = dados.PercentualConcluido;

            foreach (var etapaDados in dados.Etapas)
            {
                var etapa = obra.Etapas.FirstOrDefault(e => e.Id == etapaDados.Id);

                if (etapa != null)
                {
                    etapa.NomeEtapa = etapaDados.NomeEtapa;
                    etapa.Status = etapaDados.Status;
                }
            }

            await _context.SaveChangesAsync();

            return Ok("Obra atualizada com sucesso!");
        }
    }
}
