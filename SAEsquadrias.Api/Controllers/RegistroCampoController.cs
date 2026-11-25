using Microsoft.AspNetCore.Mvc;
using SAEsquadrias.Api.Data;
using SAEsquadrias.Api.Models;
using SAEsquadrias.Api.Dtos;

namespace SAEsquadrias.Api.Controllers
{
    [ApiController]
    [Route("api/registros")]
    public class RegistroCampoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegistroCampoController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpPost]
        public IActionResult Criar([FromForm] RegistroCampoDto dto)
        {
            var nomes = new List<string>();
            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            Directory.CreateDirectory(pasta);

            foreach (var foto in dto.Fotos)
            {
                var nome = Guid.NewGuid() + Path.GetExtension(foto.FileName);
                using var stream = System.IO.File.Create(Path.Combine(pasta, nome));
                foto.CopyTo(stream);
                nomes.Add(nome);
            }

            var registro = new RegistroCampo
            {
                ObraId = dto.ObraId,
                Status = dto.Status,
                Observacoes = dto.Observacoes,
                Fotos = string.Join(";", nomes)
            };

            _context.RegistrosCampo.Add(registro);
            _context.SaveChanges();

            return Ok(registro);
        }

        
        [HttpGet("obra/{obraId}")]
        public IActionResult ListarPorObra(int obraId)
        {
            var lista = _context.RegistrosCampo
                .Where(r => r.ObraId == obraId)
                .OrderByDescending(r => r.Data)
                .ToList();

            return Ok(lista);
        }
    }
}