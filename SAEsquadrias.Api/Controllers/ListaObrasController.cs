using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Data;
using SAEsquadrias.Api.Models;

namespace SAEsquadrias.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListaObrasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ListaObrasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListaObra>>> Get([FromQuery] string? busca, [FromQuery] string? status)
        {
            var query = _context.ListaObras.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                string termo = busca.Trim().ToLower();
                query = query.Where(o =>
                    o.NumeroProjeto.ToLower().Contains(termo) ||
                    o.Cliente.ToLower().Contains(termo) ||
                    o.Endereco.ToLower().Contains(termo)
                );
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(o => o.Status == status);
            }

            return Ok(await query.ToListAsync());
        }

      
        [HttpPost]
        public async Task<ActionResult<ListaObra>> Post(ListaObra obra)
        {
            _context.ListaObras.Add(obra);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = obra.Id }, obra);
        }
    }
}