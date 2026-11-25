using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAEsquadrias.Api.Models
{
    public class Pendencia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; } = string.Empty;
  
        [Required]
        public string Status { get; set; } = "Aberta";

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        
        [Required]
        public int ProjetoId { get; set; }

        [ForeignKey("ProjetoId")]
        public Projeto? Projeto { get; set; }

        [Required]
        public int VendedorId { get; set; }

        [ForeignKey("VendedorId")]
        public Vendedor? Vendedor { get; set; }
    }
}
