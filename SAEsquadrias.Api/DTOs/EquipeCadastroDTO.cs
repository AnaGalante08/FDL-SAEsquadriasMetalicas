using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.API.DTOs
{
    public class EquipeCadastroDTO
    {
        [Required]
        public string NomeEquipe { get; set; }

        [Required]
        public string Responsavel { get; set; }

        [Required]
        public int QuantidadeIntegrantes { get; set; }
    }
}
