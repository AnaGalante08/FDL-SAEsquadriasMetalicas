using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.Api.Models
{
    public class ListaObra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NumeroProjeto { get; set; } = string.Empty; // Ex: "#0015"

        [Required]
        public string Cliente { get; set; } = string.Empty;

        [Required]
        public string Endereco { get; set; } = string.Empty;

        [Required]
        public string Vendedor { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Aguardando liberação";

        public string Prazo { get; set; } = "-";
    }
}
