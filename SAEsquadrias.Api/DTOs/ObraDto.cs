using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.Api.Dtos
{
    public class ObraDto
    {
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        public string Cep { get; set; } = string.Empty;

        [Required(ErrorMessage = "A rua é obrigatória.")]
        public string Rua { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número é obrigatório.")]
        public string Numero { get; set; } = string.Empty;

        public string? Complemento { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado é obrigatório.")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "O cliente é obrigatório.")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "O vendedor é obrigatório.")]
        public int VendedorId { get; set; }

        [Required(ErrorMessage = "A equipe é obrigatória.")]
        public string Equipe { get; set; } = string.Empty;

        [Required(ErrorMessage = "O prazo é obrigatório.")]
        public DateTime Prazo { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        public string Status { get; set; } = string.Empty;

        public string? Observacoes { get; set; }

        public List<ItemObraDto> ProdutosUtilizados { get; set; } = new();
    }

    public class ItemObraDto
    {
        [Required(ErrorMessage = "O produto é obrigatório.")]
        public string Produto { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "As medidas são obrigatórias.")]
        public string Medidas { get; set; } = string.Empty;

        public string? Observacoes { get; set; }
    }
}
