using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.Api.DTOs
{
    public class AdministradorDto
    {
        [Required(ErrorMessage = "O nome do administrador é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Insira um endereço de e-mail válido.")]
        public string Email { get; set; } = string.Empty;

        public string? Telefone { get; set; }
    }
}
