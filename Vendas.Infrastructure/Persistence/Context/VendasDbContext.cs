using Microsoft.EntityFrameworkCore;
using Vendas.Domain.Pedidos;

namespace Vendas.Infrastructure.Persistence.Context;
public sealed class VendasDbContext(DbContextOptions<VendasDbContext> options) : DbContext(options)
{
    public DbSet<Pedido> Pedidos => Set<Pedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VendasDbContext).Assembly);

        modelBuilder.Entity<ItemPedido>(item =>
        {
            item.ToTable("ItensPedido");
            item.HasKey(i => i.Id);
            item.Property(p => p.Id).ValueGeneratedNever();
            item.Property<Guid>("PedidoId").IsRequired();
            item.Property(i => i.DataAtualizacao).IsRequired(false);
            item.Ignore(i => i.DomainEvents);
            item.Property(i => i.NomeProduto).IsRequired().HasMaxLength(200);
            item.Property(i => i.PrecoUnitario).HasPrecision(18, 2);
            item.Property(i => i.ValorTotal).HasPrecision(18, 2);
            item.Property(i => i.DescontoAplicado).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Pagamento>(pagamento =>
        {
            pagamento.ToTable("Pagamentos");
            pagamento.HasKey(p => p.Id);
            pagamento.Property(p => p.Id).ValueGeneratedNever();
            pagamento.Property(p => p.DataAtualizacao).IsRequired(false);
            pagamento.Ignore(p => p.DomainEvents);
            pagamento.Property(p => p.Valor).HasPrecision(18, 2);
            pagamento.Property(p => p.MetodoPagamento).HasConversion<string>().HasMaxLength(50);
            pagamento.Property(p => p.StatusPagamento).HasConversion<string>().HasMaxLength(50);
            pagamento.Property(p => p.CodigoTransacao).HasMaxLength(100);
        });
    }
}
