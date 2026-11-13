using SAEsquadrias.API.DTOs;

namespace SAEsquadrias.API.DTOs
{
    public class ObraRespostaDTO
    {
        public int Id { get; set; }

        public string NomeObra { get; set; }

        public int PercentualConcluido { get; set; }

        public List<EtapaObraRespostaDTO> Etapas { get; set; }
    }
}
