using System.ComponentModel.DataAnnotations;

namespace APIGerenciadorDeEstoques.Models
{
    public class Loja
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Endereco { get; set; }
    }
}
