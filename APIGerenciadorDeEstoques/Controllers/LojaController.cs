using APIGerenciadorDeEstoques.Context;
using APIGerenciadorDeEstoques.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIGerenciadorDeEstoques.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LojaController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public LojaController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static int id = 0;


        [HttpPost]
        public async Task<IActionResult> PostLoja([FromBody] Loja loja)
        {
            id++;
            loja.Id = id;
            _dbContext.lojas.Add(loja);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLojaPorId), new { id = loja.Id, message = "Loja criada com sucesso!" }, loja);

        }

        [HttpGet]
        public async Task<IActionResult> GetTodasLojas()
        {
            var todasAsLojas = await _dbContext.lojas.ToListAsync();
            if(todasAsLojas == null || !todasAsLojas.Any()) return NotFound(new {message = "Não há lojas cadastradas"});
            return Ok(todasAsLojas);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetLojaPorId(int id)
        {

            var loja = await _dbContext.lojas.FirstOrDefaultAsync(l => l.Id == id);
            if (loja == null) return NotFound(new { message = "Loja não encontrada." });
            return Ok(loja);

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoja(int id, [FromBody] Loja loja)
        {
            if (id != loja.Id) return BadRequest(new { message = "ID da loja não coincide." });

            loja.Id = id;
            _dbContext.Entry(loja).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LojaExists(id))
                {
                    return NotFound(new { message = "Loja não encontrada." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Loja alterada com sucesso." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoja(int id)
        {
            var loja = await _dbContext.lojas.FirstOrDefaultAsync(l => l.Id == id);
            if (loja == null) return NotFound(new { message = "Loja não encontrada." });

            _dbContext.lojas.Remove(loja);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Loja deletada com sucesso." });
        }

        private bool LojaExists(int id)
        {
            return _dbContext.lojas.Any(e => e.Id == id);
        }

    }
}
