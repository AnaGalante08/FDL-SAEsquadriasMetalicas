namespace SAEsquadrias.Api.Models
{
    public class Administrador : Cadastro
    {
        public string Email { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
