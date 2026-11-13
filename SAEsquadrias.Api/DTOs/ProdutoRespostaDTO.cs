namespace SAEsquadrias.API.DTOs
{
    public class ProdutoRespostaDTO
    {
        public int Id { get; set; }

        public string NomeProduto { get; set; }

        public string Descricao { get; set; }

        public decimal Largura { get; set; }

        public decimal Profundidade { get; set; }

        public decimal Altura { get; set; }

        public string Cor { get; set; }

        public int QuantidadeNecessaria { get; set; }
    }
}