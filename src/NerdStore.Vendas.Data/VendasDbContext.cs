﻿using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Data;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Data;

public class VendasDbContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediator;
    
    public VendasDbContext(
        DbContextOptions<VendasDbContext> options,
        IMediatorHandler mediator)
        : base(options)
    {
        _mediator = mediator;
    }
    
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoItem> PedidoItems { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }
    
    public async Task<bool> Commit()
    {
        foreach (var entity in ChangeTracker
                     .Entries()
                     .Where(entry => entry
                         .Entity
                         .GetType()
                         .GetProperty("DataCadastro") != null))
        {
            if (entity.State == EntityState.Added)
            {
                entity.Property("DataCadastro").CurrentValue = DateTime.Now;
            }

            if (entity.State == EntityState.Modified)
            {
                entity.Property("DataCadastro").IsModified = false;
            }
        }

        var sucesso = await base.SaveChangesAsync() > 0;

        if (sucesso)
        {
            await _mediator.PublicarEventos(this);
        }

        return sucesso;
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