namespace SAEsquadrias.Api.DTOs
{
    public class ListaObraDto
    {
        public string NumeroProjeto { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Vendedor { get; set; } = string.Empty;
        public string Status { get; set; } = "Aguardando liberação";
        public string Prazo { get; set; } = "-";
    }
}
