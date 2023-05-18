using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Data.Mappings;

public class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Nome)
            .IsRequired()
            .HasColumnType("varchar(250)");

        builder
            .Property(p => p.Descricao)
            .IsRequired()
            .HasColumnType("varchar(500)");

        builder
            .Property(p => p.Imagem)
            .IsRequired()
            .HasColumnType("varchar(250)");

        builder
            .OwnsOne(p => p.Dimensoes, d =>
            {
                d.Property(e => e.Altura)
                    .IsRequired()
                    .HasColumnType("int");

                d.Property(e => e.Largura)
                    .IsRequired()
                    .HasColumnType("int");

                d.Property(e => e.Profundidade)
                    .IsRequired()
                    .HasColumnType("int");
            });

        builder.ToTable("Produtos", "catalogo");
    }
}