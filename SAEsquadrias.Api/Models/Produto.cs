using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.API.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required]
        public string NomeProduto { get; set; }

        [Required]
        public string Descricao { get; set; }

        public decimal Largura { get; set; }

        public decimal Profundidade { get; set; }

        public decimal Altura { get; set; }

        [Required]
        public string Cor { get; set; }

        public int QuantidadeNecessaria { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
