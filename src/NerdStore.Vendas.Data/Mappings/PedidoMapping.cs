using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Data.Mappings;

public class PedidoMapping : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Codigo)
            .HasDefaultValueSql("NEXT VALUE FOR MinhaSequencia");

        builder
            .HasMany(p => p.PedidoItems)
            .WithOne(p => p.Pedido)
            .HasForeignKey(p => p.PedidoId);

        builder.ToTable("Pedido", "venda");
    }
}