using Microsoft.EntityFrameworkCore;
using MinhaApi.Models; // Importa seu model

namespace MinhaApi.Data
{
    public class MeuDbContext : DbContext
    {
        // O construtor é necessário para a injeção de dependência
        public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options) { }

        // Mapeia sua classe "Produto" para uma tabela "Produtos"
        public DbSet<Produto> Produtos { get; set; }
    }
}