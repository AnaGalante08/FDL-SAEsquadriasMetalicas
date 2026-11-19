using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Data;
using SAEsquadrias.Api.Models;
using SAEsquadrias.Api.Dtos;

namespace SAEsquadrias.Api.Controllers
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

    
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ObraDto req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == req.ClienteId && c.Ativo);
            if (!clienteExiste)
                return BadRequest(new { erro = "Cliente não encontrado ou inativo." });

            var vendedorExiste = await _context.Vendedores.AnyAsync(v => v.Id == req.VendedorId && v.Ativo);
            if (!vendedorExiste)
                return BadRequest(new { erro = "Vendedor não encontrado ou inativo." });

            
            var obra = new Obra
            {
                Cep = req.Cep.Trim(),
                Rua = req.Rua.Trim(),
                Numero = req.Numero.Trim(),
                Complemento = string.IsNullOrWhiteSpace(req.Complemento) ? null : req.Complemento.Trim(),
                Bairro = req.Bairro.Trim(),
                Cidade = req.Cidade.Trim(),
                Estado = req.Estado.Trim(),
                ClienteId = req.ClienteId,
                VendedorId = req.VendedorId,
                Equipe = req.Equipe.Trim(),
                Prazo = req.Prazo,
                Status = req.Status.Trim(),
                Observacoes = string.IsNullOrWhiteSpace(req.Observacoes) ? null : req.Observacoes.Trim(),
                ProdutosUtilizados = req.ProdutosUtilizados.Select(p => new ItemObra
                {
                    Produto = p.Produto.Trim(),
                    Quantidade = p.Quantidade,
                    Medidas = p.Medidas.Trim(),
                    Observacoes = string.IsNullOrWhiteSpace(p.Observacoes) ? null : p.Observacoes.Trim()
                }).ToList()
            };

            _context.Obras.Add(obra);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = obra.Id }, obra);
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Obra>>> Listar()
        {
            var obras = await _context.Obras
                .Include(o => o.Cliente)
                .Include(o => o.Vendedor)
                .Include(o => o.ProdutosUtilizados)
                .Where(o => o.Ativo)
                .ToListAsync();

            return Ok(obras);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Obra>> ObterPorId(int id)
        {
            var obra = await _context.Obras
                .Include(o => o.Cliente)
                .Include(o => o.Vendedor)
                .Include(o => o.ProdutosUtilizados)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obra == null || !obra.Ativo)
                return NotFound(new { erro = "Obra não encontrada." });

            return obra;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desativar(int id)
        {
            var obra = await _context.Obras.FindAsync(id);

            if (obra == null)
                return NotFound(new { erro = "Obra não encontrada." });

            obra.Ativo = false;
            _context.Obras.Update(obra);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}