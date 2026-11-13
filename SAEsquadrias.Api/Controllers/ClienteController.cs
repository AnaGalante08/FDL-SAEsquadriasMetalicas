using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Data;
using SAEsquadrias.Api.Models;
using SAEsquadrias.Api.Dtos; // Não te esqueças de incluir o namespace dos DTOs

namespace SAEsquadrias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ClienteRequest req)
        {
            // Valida automaticamente as regras definidas no DTO ([Required], [EmailAddress], etc.)
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = req.Email.Trim().ToLower();

            // Verifica se o e-mail já está cadastrado no banco
            var emailExiste = await _context.Clientes
                .AnyAsync(c => c.Email.ToLower() == email);

            if (emailExiste)
            {
                return Conflict(new { erro = "Já existe um cliente com este e-mail." });
            }

            // Mapeia os dados seguros do DTO para a entidade do Banco de Dados (Model)
            var cliente = new Cliente
            {
                Nome = req.Nome.Trim(),
                Email = email,
                Telefone = string.IsNullOrWhiteSpace(req.Telefone) ? null : req.Telefone.Trim()
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            // Retorna 201 Created com a localização do novo recurso
            return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id }, cliente);
        }

        // GET: api/clientes
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Cliente>>> Listar()
        {
            var clientes = await _context.Clientes
                .Where(c => c.Ativo) // Traz apenas os clientes ativos
                .ToListAsync();

            return Ok(clientes);
        }

        // GET: api/clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> ObterPorId(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound(new { erro = "Cliente não encontrado." });
            }

            return cliente;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desativar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound(new { erro = "Cliente não encontrado." });
            }

            // Exclusão lógica: apenas desativa em vez de apagar do banco
            cliente.Ativo = false;

            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204 indicando sucesso sem corpo de resposta
        }
    }
}