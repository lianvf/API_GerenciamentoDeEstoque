using System.ComponentModel.DataAnnotations;

namespace APIGerenciadorDeEstoques.ModelsAux
{
    public class EstoqueAux
    {
        [Required]
        public int IdProduto { get; set; }
        [Required]
        public int IdLoja { get; set; }
        [Required]
        public int qtdItens { get; set; }
    }
}
