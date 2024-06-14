using APIGerenciadorDeEstoques.Context;
using APIGerenciadorDeEstoques.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIGerenciadorDeEstoques.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController :ControllerBase
    {
        private readonly databaseContext _dbContext;

        public ProdutoController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static int id = 0;
        

        [HttpPost]
        public async Task<IActionResult> PostProduto([FromBody] Produto produto)
        {
            id++;
            produto.Id = id;
            _dbContext.produtos.Add(produto);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProdutoPorId), new { id = produto.Id, message = "Produto criado com sucesso!" }, produto);

        }

        [HttpGet]
        public async Task<IActionResult> GetTodosProdutos()
        {
            var todosOsProdutos = await _dbContext.produtos.ToListAsync();
            if (todosOsProdutos == null || !todosOsProdutos.Any()) return NotFound(new { message = "Não há produtos cadastrados" });
            return Ok(todosOsProdutos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProdutoPorId(int id)
        {
           
             var produtoEmEstoque = await _dbContext.produtos.FirstOrDefaultAsync(p => p.Id == id);
            if (produtoEmEstoque == null) return NotFound(new { message = "Produto não encontrado." });
            return Ok(produtoEmEstoque);

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, [FromBody] Produto produto)
        {
            if (id != produto.Id) return BadRequest(new { message = "ID do produto não coincide." });

            produto.Id = id;
            _dbContext.Entry(produto).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound(new { message = "Produto não encontrado." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Produto alterado com sucesso." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produtoEmEstoque = await _dbContext.produtos.FirstOrDefaultAsync(p => p.Id == id);
            if (produtoEmEstoque == null) return NotFound(new { message = "Produto não encontrado." });

            _dbContext.produtos.Remove(produtoEmEstoque);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Produto deletado com sucesso." });
        }

        private bool ProdutoExists(int id)
        {
            return _dbContext.produtos.Any(e => e.Id == id);
        }

    }
}
