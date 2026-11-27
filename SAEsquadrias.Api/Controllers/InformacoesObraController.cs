using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Data;
using SAEsquadrias.Api.DTOs;

namespace SAEsquadrias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformacoesObraController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InformacoesObraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InformacoesObrasDto>> GetInformacoesObras(int id)
        {
            var obra = await _context.Obras
                .Include(o => o.Cliente)
                .Include(o => o.Vendedor)
                .Include(o => o.ProdutosUtilizados)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obra == null)
            {
                return NotFound(new { mensagem = "Obra não encontrada." });
            }

            var partesEndereco = new[]
            {
                string.IsNullOrWhiteSpace(obra.Numero) ? obra.Rua : $"{obra.Rua}, {obra.Numero}",
                obra.Complemento,
                obra.Bairro,
                obra.Cidade,
                obra.Estado,
                obra.Cep
            }.Where(p => !string.IsNullOrWhiteSpace(p));

            var dto = new InformacoesObrasDto
            {
                Id = obra.Id,
                Status = obra.Status,
                Endereco = string.Join(", ", partesEndereco),
                VendedorNome = obra.Vendedor?.Nome ?? "Não informado",
                PrazoEntrega = obra.Prazo.ToString("dd/MM/yyyy"),
                ObservacoesGerais = obra.Observacoes,

                ClienteNome = obra.Cliente?.Nome ?? "Não informado",
                ClienteTelefone = obra.Cliente?.Telefone ?? "",
                ClienteEmail = obra.Cliente?.Email ?? "",

                Produtos = obra.ProdutosUtilizados.Select(p => new ProdutoProjetoDto
                {
                    Id = p.Id,
                    Produto = p.Produto,
                    Medidas = p.Medidas,
                    Observacoes = p.Observacoes
                }).ToList()
            };

            return Ok(dto);
        }
    }
}