namespace SAEsquadrias.Api.DTOs
{
    public class PendenciaDto
    {
        public int Id { get; set; }
        public string CodigoProjeto { get; set; } = string.Empty; 
        public string Descricao { get; set; } = string.Empty;
        public string ResponsavelNome { get; set; } = string.Empty; 
        public string Status { get; set; } = string.Empty;
    }

    public class PendenciaRequestDto
    {
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = "Aberta";
        public int ObraId { get; set; }
        public int VendedorId { get; set; }
    }
}

