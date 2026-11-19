using Microsoft.AspNetCore.Mvc;
using SAEsquadrias.Api.Data;
using SAEsquadrias.Api.Dtos;
using SAEsquadrias.Api.Models;

namespace SAEsquadrias.Api.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Cadastrar(CadastroUsuarioDto dto)
        {
            bool existeVendedor = _context.Usuarios.Any(u => u.Perfil == "VENDEDOR");

            if (!existeVendedor)
            {
                
                if (dto.Perfil != "VENDEDOR")
                    return BadRequest(new { erro = "Cadastre primeiro um VENDEDOR." });
            }
            else
            {
                var quemCadastra = _context.Usuarios.FirstOrDefault(u => u.Id == dto.UsuarioLogadoId);

                if (quemCadastra == null || quemCadastra.Perfil != "VENDEDOR")
                    return StatusCode(403, new { erro = "Só o vendedor pode cadastrar." });

                if (dto.Perfil != "VENDEDOR" && dto.Perfil != "CLIENTE" && dto.Perfil != "EQUIPE_EXTERNA")
                    return BadRequest(new { erro = "Perfil inválido. Use VENDEDOR, CLIENTE ou EQUIPE_EXTERNA." });
            }

            if (_context.Usuarios.Any(u => u.Email == dto.Email))
                return BadRequest(new { erro = "Já existe um usuário com esse e-mail." });

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha,
                Perfil = dto.Perfil
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok(new { id = usuario.Id, nome = usuario.Nome, perfil = usuario.Perfil });
        }
    }
}