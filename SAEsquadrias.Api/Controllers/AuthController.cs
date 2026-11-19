using Microsoft.AspNetCore.Mvc;
using SAEsquadrias.Api.Data;
using SAEsquadrias.Api.Dtos;

namespace SAEsquadrias.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == dto.Email && u.Senha == dto.Senha);

            if (usuario == null)
                return Unauthorized(new { erro = "E-mail ou senha inválidos." });

            if (usuario.Perfil != "VENDEDOR")
                return StatusCode(403, new { erro = "Acesso permitido somente para vendedores." });

            return Ok(new LoginRespostaDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Perfil = usuario.Perfil
            });
        }
    }
}