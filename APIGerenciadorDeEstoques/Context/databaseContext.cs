using APIGerenciadorDeEstoques.Models;
using Microsoft.EntityFrameworkCore;

namespace APIGerenciadorDeEstoques.Context
{
    public class databaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "GerenciamentoEstoqueDb");
        }

        public DbSet<Produto> produtos { get; set; }
        public DbSet<Loja> lojas { get; set; }
        public DbSet<Estoque> estoques { get; set; }

    }
}
