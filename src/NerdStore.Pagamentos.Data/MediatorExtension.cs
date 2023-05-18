using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Pagamentos.Data;

public static class MediatorExtension
{
    public static async Task PublicarEventos(this IMediatorHandler mediatorHandler, PagamentoContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.Notificacoes.Any());

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.Notificacoes)
            .ToList();
        
        domainEntities
            .ToList()
            .ForEach(entity => entity.Entity.LimparEventos());

        var tasks = domainEvents
            .Select(async (domainEvent) =>
            {
                await mediatorHandler.PublicarEvento(domainEvent);
            });

        await Task.WhenAll(tasks);
    }
}