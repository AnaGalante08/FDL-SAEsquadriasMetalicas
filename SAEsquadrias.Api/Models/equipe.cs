using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.API.Models
{
    public class Equipe
    {
        public int Id { get; set; }

        [Required]
        public string NomeEquipe { get; set; }

        [Required]
        public string Responsavel { get; set; }

        [Required]
        public int QuantidadeIntegrantes { get; set; }

        public bool Ativo { get; set; } = true;
    }
}