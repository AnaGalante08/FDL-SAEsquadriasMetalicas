namespace SAEsquadrias.Api.Dtos
{
    public class CadastroUsuarioDto
    {
        public int UsuarioLogadoId { get; set; } 
        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
        public string Senha { get; set; } = "";
        public string Perfil { get; set; } = "";
    }
}