namespace SAEsquadrias.Api.Models
{
    public class RegistroCampo
    {
        public int Id { get; set; }
        public int ObraId { get; set; }                
        public string Fotos { get; set; } = "";        
        public string Status { get; set; } = "";      
        public string? Observacoes { get; set; }   
        public DateTime Data { get; set; } = DateTime.Now;
    }
}