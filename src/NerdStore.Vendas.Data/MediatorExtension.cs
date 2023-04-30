using NerdStore.Core.Bus;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Data;

public static class MediatorExtension
{
    public static async Task PublicarEventos(this IMediatrHandler mediatorHandler, VendasDbContext context)
    {
        var domainEntities = context
            .ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.Notificacoes.Any());

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.Notificacoes)
            .ToList();
        
        domainEntities
            .ToList()
            .ForEach(e => e.Entity.LimparEventos());

        var tasks = domainEvents
            .Select(async domainEvent =>
            {
                await mediatorHandler.PublicarEvento(domainEvent);
            });

        await Task.WhenAll(tasks);
    }    
}