using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Data;

public class VendasDbContext : DbContext, IUnitOfWork
{
    public VendasDbContext(
        DbContextOptions<VendasDbContext> options)
        : base(options)
    {
        
    }

    public async Task<bool> Commit()
    {
        return true;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder
                     .Model
                     .GetEntityTypes()
                     .SelectMany(e => e
                         .GetProperties()
                         .Where(p => p.ClrType == typeof(string))))
        {
            property.SetColumnType("varchar(100)");
        }

        modelBuilder.Ignore<Event>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VendasDbContext).Assembly);

        foreach (var relationship in modelBuilder
                     .Model
                     .GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        modelBuilder
            .HasSequence<int>("MinhaSequencia")
            .StartsAt(1000)
            .IncrementsBy(1);
        
        base.OnModelCreating(modelBuilder);
    }
}