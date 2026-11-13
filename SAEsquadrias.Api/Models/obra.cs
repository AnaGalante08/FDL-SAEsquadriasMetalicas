using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.API.Models
{
    public class Obra
    {
        public int Id { get; set; }

        [Required]
        public string NomeObra { get; set; }

        public int PercentualConcluido { get; set; }

        public List<EtapaObra> Etapas { get; set; } = new List<EtapaObra>();
    }
}