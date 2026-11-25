namespace SAEsquadrias.Api.Models
{
    public class Obra
    {
        public int Id { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

        
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int VendedorId { get; set; }
        public Vendedor? Vendedor { get; set; }

        public string Equipe { get; set; } = string.Empty;

        public DateTime Prazo { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; }

        public bool Ativo { get; set; } = true;

        
        public List<ItemObra> ProdutosUtilizados { get; set; } = new();
    }

    public class ItemObra
    {
        public int Id { get; set; }
        public int ObraId { get; set; }
        public string Produto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public string Medidas { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
    }
}
