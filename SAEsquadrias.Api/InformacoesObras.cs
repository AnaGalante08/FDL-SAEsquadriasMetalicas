using SAEsquadrias.Api.Models;

namespace SAEsquadrias.Api
{
    public class InformacoesObras
    {
        public int Id { get; set; }
        public string Produto { get; set; } = string.Empty; 
        public string Localizacao { get; set; } = string.Empty; 
        public string Medidas { get; set; } = string.Empty; 
        public string? Observacoes { get; set; }

        public int ObraId { get; set; }
        public Obra? Obra { get; set; }
    }
}
