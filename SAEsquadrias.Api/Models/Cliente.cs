namespace SAEsquadrias.Api.Models
{
    public class Cliente : Cadastro
    {
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
