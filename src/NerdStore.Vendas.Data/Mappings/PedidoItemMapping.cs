using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Data.Mappings;

public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItem>
{
    public void Configure(EntityTypeBuilder<PedidoItem> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.ProdutoNome)
            .HasColumnType("varchar(250)");

        builder
            .HasOne(p => p.Pedido)
            .WithMany(p => p.PedidoItems);

        builder.ToTable("PedidoItems", "venda");
    }
}