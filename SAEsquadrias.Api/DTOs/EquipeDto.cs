using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.Api.DTOs
{
    public class EquipeDto
    {

            [Required(ErrorMessage = "O nome da equipe é obrigatório.")]
            [MaxLength(120)]
            public string Nome { get; set; } = string.Empty;

            [Required(ErrorMessage = "O responsável pela equipe é obrigatório.")]
            [MaxLength(120)]
            public string Responsavel { get; set; } = string.Empty;

            [Required(ErrorMessage = "A quantidade de integrantes é obrigatória.")]
            [Range(1, int.MaxValue, ErrorMessage = "A quantidade de integrantes deve ser maior que zero.")]
            public int QuantidadeIntegrantes { get; set; }
        
    }
}

