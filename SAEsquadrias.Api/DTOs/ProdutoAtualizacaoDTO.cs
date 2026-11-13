
using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.API.DTOs
{
    public class ProdutoAtualizacaoDTO
    {
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
    }
}
