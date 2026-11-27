namespace SAEsquadrias.Api.Models
{
    public class EtapaObra
    {
        public int Id { get; set; }

        public int ObraId { get; set; }
        public Obra? Obra { get; set; }

        public string Nome { get; set; } = string.Empty;

        public int Ordem { get; set; }

        public string Status { get; set; } = "Pendente";

        public DateTime? DataConclusao { get; set; }
    }
}
