using MediatR;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Data;
using NerdStore.Catalogo.Data.Repository;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Application.Queries;
using NerdStore.Vendas.Data;
using NerdStore.Vendas.Data.Repository;
using NerdStore.Vendas.Domain;

namespace NerdStore.Catalogo.WebApp.MVC.Setup;

public static class DependecyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        services.AddScoped<IProdutoRepository, ProdutoRepository>();

        services.AddScoped<IProdutoAppService, ProdutoAppService>();
        services.AddScoped<IEstoqueService, EstoqueService>();
        
        //Vendas
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IPedidoQueries, PedidoQueries>();

        services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<RemoverItemPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<AplicarVoucherPedidoCommand, bool>, PedidoCommandHandler>();
        
        services.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();

        services.AddScoped<CatalogoContext>();
        services.AddScoped<VendasDbContext>();

        services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();
    }    
}