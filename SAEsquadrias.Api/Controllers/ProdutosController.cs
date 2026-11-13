using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadrias.API.Data;
using SAEsquadrias.API.DTOs;
using SAEsquadrias.API.Models;

namespace SAEsquadrias.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

    
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var produtos = await _context.Produtos
                .Where(p => p.Ativo == true)
                .ToListAsync();

            var resposta = produtos.Select(p => new ProdutoRespostaDTO
            {
                Id = p.Id,
                NomeProduto = p.NomeProduto,
                Descricao = p.Descricao,
                Largura = p.Largura,
                Profundidade = p.Profundidade,
                Altura = p.Altura,
                Cor = p.Cor,
                QuantidadeNecessaria = p.QuantidadeNecessaria
            });

            return Ok(resposta);
        }

    
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == id && p.Ativo == true);

            if (produto == null)
            {
                return NotFound();
            }

            var resposta = new ProdutoRespostaDTO
            {
                Id = produto.Id,
                NomeProduto = produto.NomeProduto,
                Descricao = produto.Descricao,
                Largura = produto.Largura,
                Profundidade = produto.Profundidade,
                Altura = produto.Altura,
                Cor = produto.Cor,
                QuantidadeNecessaria = produto.QuantidadeNecessaria
            };

            return Ok(resposta);
        }

      
        [HttpPost]
        public async Task<IActionResult> Post(ProdutoCadastroDTO dados)
        {
            var produto = new Produto
            {
                NomeProduto = dados.NomeProduto,
                Descricao = dados.Descricao,
                Largura = dados.Largura,
                Profundidade = dados.Profundidade,
                Altura = dados.Altura,
                Cor = dados.Cor,
                QuantidadeNecessaria = dados.QuantidadeNecessaria,
                Ativo = true
            };

            _context.Produtos.Add(produto);

            await _context.SaveChangesAsync();

            return Ok("Produto cadastrado com sucesso!");
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProdutoAtualizacaoDTO dados)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == id && p.Ativo == true);

            if (produto == null)
            {
                return NotFound();
            }

            produto.NomeProduto = dados.NomeProduto;
            produto.Descricao = dados.Descricao;
            produto.Largura = dados.Largura;
            produto.Profundidade = dados.Profundidade;
            produto.Altura = dados.Altura;
            produto.Cor = dados.Cor;
            produto.QuantidadeNecessaria = dados.QuantidadeNecessaria;

            await _context.SaveChangesAsync();

            return Ok("Produto atualizado com sucesso!");
        }

       

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null || produto.Ativo == false)
            {
                return NotFound();
            }

            produto.Ativo = false;

            await _context.SaveChangesAsync();

            return Ok("Produto desativado com sucesso!");
        }
    }
}
