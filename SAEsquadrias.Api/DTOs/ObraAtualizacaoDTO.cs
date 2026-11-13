using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.API.DTOs
{
    public class ObraAtualizacaoDTO
    {
        [Required]
        public string NomeObra { get; set; }

        public int PercentualConcluido { get; set; }

        public List<EtapaObraRespostaDTO> Etapas { get; set; }
    }
}
