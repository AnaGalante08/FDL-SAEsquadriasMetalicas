using System.ComponentModel.DataAnnotations;

namespace SAEsquadrias.Api.Models
{
    public abstract class Cadastro
    {
        public int Id { get; set; }

        [MaxLength(120)]
        public string Nome { get; set; } = string.Empty;
    }
    public class Equipe : Cadastro { }
}
