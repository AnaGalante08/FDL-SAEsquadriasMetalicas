namespace SAEsquadrias.Api.Dtos
{
    public class RegistroCampoDto
    {
        public int ObraId { get; set; }
        public string Status { get; set; } = "";
        public string? Observacoes { get; set; }
        public List<IFormFile> Fotos { get; set; } = new();
    }
}