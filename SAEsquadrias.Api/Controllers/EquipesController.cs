using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadrias.API.Data;
using SAEsquadrias.API.Models;
using SAEsquadrias.API.DTOs;

namespace SAEsquadrias.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipesController(AppDbContext context)
        {
            _context = context;
        }

   
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var equipes = await _context.Equipes
                .Where(e => e.Ativo == true)
                .ToListAsync();

            var resposta = equipes.Select(e => new EquipeRespostaDTO
            {
                Id = e.Id,
                NomeEquipe = e.NomeEquipe,
                Responsavel = e.Responsavel,
                QuantidadeIntegrantes = e.QuantidadeIntegrantes
            });

            return Ok(resposta);
        }

      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var equipe = await _context.Equipes.FindAsync(id);

            if (equipe == null || equipe.Ativo == false)
            {
                return NotFound();
            }

            var resposta = new EquipeRespostaDTO
            {
                Id = equipe.Id,
                NomeEquipe = equipe.NomeEquipe,
                Responsavel = equipe.Responsavel,
                QuantidadeIntegrantes = equipe.QuantidadeIntegrantes
            };

            return Ok(resposta);
        }

        
        [HttpPost]
        public async Task<IActionResult> Post(EquipeCadastroDTO dados)
        {
            var equipe = new Equipe
            {
                NomeEquipe = dados.NomeEquipe,
                Responsavel = dados.Responsavel,
                QuantidadeIntegrantes = dados.QuantidadeIntegrantes,
                Ativo = true
            };

            _context.Equipes.Add(equipe);

            await _context.SaveChangesAsync();

            return Ok(equipe);
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EquipeAtualizacaoDTO dados)
        {
            var equipe = await _context.Equipes.FindAsync(id);

            if (equipe == null || equipe.Ativo == false)
            {
                return NotFound();
            }

            equipe.NomeEquipe = dados.NomeEquipe;
            equipe.Responsavel = dados.Responsavel;
            equipe.QuantidadeIntegrantes = dados.QuantidadeIntegrantes;

            await _context.SaveChangesAsync();

            return Ok(equipe);
        }

    
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var equipe = await _context.Equipes.FindAsync(id);

            if (equipe == null || equipe.Ativo == false)
            {
                return NotFound();
            }

            equipe.Ativo = false;

            await _context.SaveChangesAsync();

            return Ok("Equipe desativada com sucesso!");
        }
    }
}
