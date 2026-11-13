using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.API.Models
{
    public class EtapaObra
    {
        public int Id { get; set; }

        [Required]
        public string NomeEtapa { get; set; }

        [Required]
        public string Status { get; set; }

        public int ObraId { get; set; }

        public Obra Obra { get; set; }
    }
}