using AvaliacaoDev.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AvaliacaoDev.Infrastructure.Data
{
    // Contexto do banco de dados usando Entity Framework Core.
    // Configurado para usar banco em memória para simplificação.
    public class AppDbContext : DbContext
    {
        // Construtor que recebe as opções de configuração do DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        
        // DbSet para acesso aos pedidos
        public DbSet<Pedido> Pedidos { get; set; }
        
        // DbSet para acesso aos produtos
        public DbSet<Produto> Produtos { get; set; }
        
        // DbSet para acesso aos clientes
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relacionamento Pedido -> Cliente
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey("ClienteId");

            // Configurar relacionamento Pedido -> Produto (muitos-para-muitos)
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Produtos)
                .WithMany();
        }
    }
}