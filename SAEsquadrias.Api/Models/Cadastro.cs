using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.Api.Models
{
    public abstract class Cadastro
    {
        public int Id { get; set; }

        [MaxLength(120)]
        public string Nome { get; set; } = string.Empty;
    }
    public class Equipe : Cadastro 
    {
        [Required]
        [MaxLength(120)]
        public string Responsavel { get; set; } = string.Empty;

        [Required]
        public int QuantidadeIntegrantes { get; set; }
    }
}
