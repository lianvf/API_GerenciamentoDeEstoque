using System.ComponentModel.DataAnnotations;

namespace APIGerenciadorDeEstoques.Models
{
    public class Produto
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public decimal PrecoCusto { get; set; }
    }
}
