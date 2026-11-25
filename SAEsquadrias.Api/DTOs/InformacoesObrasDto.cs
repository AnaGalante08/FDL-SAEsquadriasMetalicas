namespace SAEsquadrias.Api.DTOs
{
    public class InformacoesObrasDto
    {
        public int Id { get; set; }
        public string CodigoFormatado => $"#{Id:D4}"; 
        public string Status { get; set; } = string.Empty; 

        public string Endereco { get; set; } = string.Empty;
        public string VendedorNome { get; set; } = string.Empty;
        public string PrazoEntrega { get; set; } = string.Empty;
        public string? ObservacoesGerais { get; set; }

        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public string ClienteEmail { get; set; } = string.Empty;

        public List<ProdutoProjetoDto> Produtos { get; set; } = new();
    }

    public class ProdutoProjetoDto
    {
        public int Id { get; set; }
        public string Produto { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;
        public string Medidas { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
    }
}

