using APIGerenciadorDeEstoques.Context;
using APIGerenciadorDeEstoques.Models;
using APIGerenciadorDeEstoques.ModelsAux;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIGerenciadorDeEstoques.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public EstoqueController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static int id = 0;


        [HttpPost]
        public async Task<IActionResult> PostEstoque([FromBody] EstoqueAux estoqueAux)
        {
            var loja = await _dbContext.lojas.FirstOrDefaultAsync(l => l.Id == estoqueAux.IdLoja);
            if (loja == null)
            {
                return NotFound(new { message = "Loja não encontrada. Por favor, selecione uma loja válida ou cadastre uma nova loja." });
            }

            var produto = await _dbContext.produtos.FirstOrDefaultAsync(p => p.Id == estoqueAux.IdProduto);
            if (produto == null)
            {
                return NotFound(new { message = "Produto não encontrado. Por favor, selecione um produto válido ou cadastre um novo produto." });
            }

            var estoqueExistente = await _dbContext.estoques.FirstOrDefaultAsync(e => e.idProduto == produto.Id && e.idLoja == loja.Id);
            if (estoqueExistente == null)
            {
                // Se não existir, criar um novo registro de estoque
                Estoque novoEstoque = new Estoque
                {
                    idProduto = produto.Id,
                    idLoja = loja.Id,
                    qtdItens = estoqueAux.qtdItens
                };

                _dbContext.estoques.Add(novoEstoque);
                await _dbContext.SaveChangesAsync();

                // Retorna apenas as informações necessárias
                return CreatedAtAction(nameof(GetEstoquePorId), new { id = novoEstoque.Id, message = "Estoque criado com sucesso!" }, new
                {
                    novoEstoque.Id,
                    novoEstoque.idProduto,
                    novoEstoque.idLoja,
                    novoEstoque.qtdItens
                });
            }
            else
            {
                // Se existir, apenas atualizar a quantidade
                estoqueExistente.qtdItens += estoqueAux.qtdItens;
                _dbContext.estoques.Update(estoqueExistente);
                await _dbContext.SaveChangesAsync();

                // Retorna apenas as informações necessárias
                return Ok(new
                {
                    message = "Estoque atualizado com sucesso!",
                    estoque = new
                    {
                        estoqueExistente.Id,
                        estoqueExistente.idProduto,
                        estoqueExistente.idLoja,
                        estoqueExistente.qtdItens
                    }
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetTodosEstoques()
        {
            var todosOsEstoques = await _dbContext.estoques.ToListAsync();

            if (todosOsEstoques == null || !todosOsEstoques.Any()) return NotFound(new { message = "Não há estoques cadastrados" });

            var resultado = new List<object>();

            foreach (var estoque in todosOsEstoques)
            {
                var produto = await _dbContext.produtos.FirstOrDefaultAsync(p => p.Id == estoque.idProduto);
                var loja = await _dbContext.lojas.FirstOrDefaultAsync(l => l.Id == estoque.idLoja);

                if (produto != null && loja != null)
                {
                    resultado.Add(new
                    {
                        estoque.Id,
                        estoque.idProduto,
                        ProdutoNome = produto.Nome,
                        ProdutoPrecoCusto = produto.PrecoCusto,
                        estoque.idLoja,
                        LojaNome = loja.Nome,
                        estoque.qtdItens
                    });
                }
            }

            return Ok(resultado);
        }

        //Procura pelo id do estoque
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEstoquePorId(int id)
        {
            var estoque = await _dbContext.estoques.FirstOrDefaultAsync(e => e.Id == id);
            if (estoque == null) return NotFound(new { message = "Estoque não encontrado." });

            var produto = await _dbContext.produtos.FirstOrDefaultAsync(p => p.Id == estoque.idProduto);
            if (produto == null) return NotFound(new { message = "Produto não encontrado." });

            var loja = await _dbContext.lojas.FirstOrDefaultAsync(l => l.Id == estoque.idLoja);
            if (loja == null) return NotFound(new { message = "Loja não encontrada." });

            var resultado = new
            {
                estoque.Id,
                estoque.idProduto,
                ProdutoNome = produto.Nome,
                ProdutoPrecoCusto = produto.PrecoCusto,
                estoque.idLoja,
                LojaNome = loja.Nome,
                estoque.qtdItens
            };

            return Ok(resultado);
        }


        //Procura pelo id do produto
        [HttpGet("produto/{idProduto}")]
        public async Task<IActionResult> GetEstoquesPorIdProduto(int idProduto)
        {
            var estoques = await _dbContext.estoques.Where(e => e.idProduto == idProduto).ToListAsync();

            if (estoques == null || !estoques.Any()) return NotFound(new { message = "Não há estoques cadastrados para este produto" });

            var resultado = new List<object>();

            foreach (var estoque in estoques)
            {
                var produto = await _dbContext.produtos.FirstOrDefaultAsync(p => p.Id == estoque.idProduto);
                var loja = await _dbContext.lojas.FirstOrDefaultAsync(l => l.Id == estoque.idLoja);

                if (produto != null && loja != null)
                {
                    resultado.Add(new
                    {
                        estoque.Id,
                        estoque.idProduto,
                        ProdutoNome = produto.Nome,
                        ProdutoPrecoCusto = produto.PrecoCusto,
                        estoque.idLoja,
                        LojaNome = loja.Nome,
                        estoque.qtdItens
                    });
                }
            }

            return Ok(resultado);
        }

        //Procura pelo id da loja
        [HttpGet("loja/{idLoja}")]
        public async Task<IActionResult> GetEstoquesPorIdLoja(int idLoja)
        {
            var estoques = await _dbContext.estoques.Where(e => e.idLoja == idLoja).ToListAsync();

            if (estoques == null || !estoques.Any()) return NotFound(new { message = "Não há estoques cadastrados para esta loja" });

            var resultado = new List<object>();

            foreach (var estoque in estoques)
            {
                var produto = await _dbContext.produtos.FirstOrDefaultAsync(p => p.Id == estoque.idProduto);
                var loja = await _dbContext.lojas.FirstOrDefaultAsync(l => l.Id == estoque.idLoja);

                if (produto != null && loja != null)
                {
                    resultado.Add(new
                    {
                        estoque.Id,
                        estoque.idProduto,
                        ProdutoNome = produto.Nome,
                        ProdutoPrecoCusto = produto.PrecoCusto,
                        estoque.idLoja,
                        LojaNome = loja.Nome,
                        estoque.qtdItens
                    });
                }
            }

            return Ok(resultado);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstoque(int id, [FromBody] EstoqueAux estoque)
        {
            var estoqueExistente = await _dbContext.estoques.FindAsync(id);
            if (estoqueExistente == null) return NotFound(new { message = "Estoque não encontrado." });

            estoqueExistente.idProduto = estoque.IdProduto;
            estoqueExistente.idLoja = estoque.IdLoja;
            estoqueExistente.qtdItens = estoque.qtdItens;

            _dbContext.Entry(estoqueExistente).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstoqueExists(id))
                {
                    return NotFound(new { message = "Estoque não encontrado." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Estoque alterado com sucesso." });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstoque (int id)
        {
            var estoque = await _dbContext.estoques.FirstOrDefaultAsync(e => e.Id == id);
            if (estoque == null) return NotFound(new { message = "Estoque não encontrado." });

            _dbContext.estoques.Remove(estoque);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Estoque deletado com sucesso." });
        }

        private bool EstoqueExists(int id)
        {
            return _dbContext.estoques.Any(e => e.Id == id);
        }

    }
}
