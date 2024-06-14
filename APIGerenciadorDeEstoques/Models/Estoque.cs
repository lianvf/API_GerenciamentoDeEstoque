using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGerenciadorDeEstoques.Models
{
    public class Estoque
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Loja")]
        public int idLoja { get; set; }
        public Loja loja { get; set; }

        [Required]
        [ForeignKey("Produto")]
        public int idProduto { get; set; }
        public Produto produto { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int qtdItens { get; set; }
    }
}
