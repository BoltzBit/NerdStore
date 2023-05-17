using MediatR;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Dto;
using NerdStore.Core.Extensions;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Commands;

public class PedidoCommandHandler : 
    IRequestHandler<AdicionarItemPedidoCommand, bool>,
    IRequestHandler<AtualizarItemPedidoCommand, bool>,
    IRequestHandler<RemoverItemPedidoCommand, bool>,
    IRequestHandler<AplicarVoucherPedidoCommand, bool>,
    IRequestHandler<IniciarPedidoCommand, bool>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IMediatorHandler _mediator;

    public PedidoCommandHandler(
        IPedidoRepository pedidoRepository,
        IMediatorHandler mediator)
    {
        _pedidoRepository = pedidoRepository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(
        AdicionarItemPedidoCommand message,
        CancellationToken cancellationToken)
    {
        if (!ValidarComando(message)) return false;

        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
        var pedidoItem = new PedidoItem(
            message.ProdutoId,
            message.Nome,
            message.Quantidade,
            message.ValorUnitario);

        if (pedido is null)
        {
            pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
            pedido.AdicionarItem(pedidoItem);
            
            _pedidoRepository.Adicionar(pedido);
            
            pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(
                message.ClienteId,
                pedido.Id));
        }
        else
        {
            var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
            pedido.AdicionarItem(pedidoItem);

            if (pedidoItemExistente)
            {
                _pedidoRepository
                    .AtualizarItem(pedido
                        .PedidoItems
                        .FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
            }
            else
            {
                _pedidoRepository.AdicionarItem(pedidoItem);
            }
            
            pedido.AdicionarEvento(new PedidoAtualizadoEvent(
                message.ClienteId,
                pedido.Id,
                pedido.ValorTotal));
        }
        
        pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(
            message.ClienteId,
            pedido.Id,
            message.ProdutoId,
            message.Nome,
            message.ValorUnitario,
            message.Quantidade));

        return await _pedidoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(
        AtualizarItemPedidoCommand message,
        CancellationToken cancellationToken)
    {
        if (!ValidarComando(message)) return false;

        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);

        if (pedido is null)
        {
            await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Pedido nao econtrado!"));
            return false;
        }

        var pedidoItem = await _pedidoRepository.ObterItemPorPedido(pedido.Id, message.ProdutoId);

        if (!pedido.PedidoItemExistente(pedidoItem))
        {
            await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Pedido nao encontrado"));
            return false;
        }
        
        pedido.AtualizarUnidades(pedidoItem, message.Quantidade);

        pedido.AdicionarEvento(new PedidoAtualizadoEvent(
            pedido.ClienteId,
            pedido.Id,
            pedido.ValorTotal));
        
        pedido.AdicionarEvento(new PedidoProdutoAtualizadoEvent(
            message.ClienteId,
            pedido.Id,
            message.ProdutoId,
            message.Quantidade));
        
        _pedidoRepository.AtualizarItem(pedidoItem);
        _pedidoRepository.AtualizarPedido(pedido);

        return await _pedidoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(
        RemoverItemPedidoCommand message,
        CancellationToken cancellationToken)
    {
        if (!ValidarComando(message)) return false;

        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);

        if (pedido is null)
        {
            await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Pedido nao encontrado!"));
            return false;
        }

        var pedidoItem = await _pedidoRepository.ObterItemPorPedido(pedido.Id, message.ProdutoId);

        if (pedidoItem is not null && pedido.PedidoItemExistente(pedidoItem))
        {
            await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Item do pedido nao encontrado!"));
            return false;
        }
        
        pedido.RemoverItem(pedidoItem);
        
        pedido.AdicionarEvento(new PedidoAtualizadoEvent(
            pedido.ClienteId,
            pedido.Id,
            pedido.ValorTotal));
        
        pedido.AdicionarEvento(new PedidoProdutoRemovidoEvent(
            message.ClienteId,
            pedido.Id,
            message.ProdutoId));
        
        _pedidoRepository.RemoverItem(pedidoItem);
        _pedidoRepository.AtualizarPedido(pedido);

        return await _pedidoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(
        AplicarVoucherPedidoCommand message,
        CancellationToken cancellationToken)
    {
        if (!ValidarComando(message)) return false;

        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);

        if (pedido is null)
        {
            await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Pedido nao encontrado!"));
            return false;
        }

        var voucher = await _pedidoRepository.ObterVoucherPorCodigo(message.CodigoVoucher);

        if (voucher is null)
        {
            await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Voucher nao encontrado!"));
            return false;
        }

        var voucherAplicavelValidation = pedido.AplicarVoucher(voucher);

        if (!voucherAplicavelValidation.IsValid)
        {
            foreach (var error in voucherAplicavelValidation.Errors)
            {
                await _mediator.PublicarNotificacao(new DomainNotification(error.ErrorCode, error.ErrorMessage));
            }

            return false;
        }
        
        pedido.AdicionarEvento(new PedidoAtualizadoEvent(
            pedido.ClienteId,
            pedido.Id,
            pedido.ValorTotal));
        
        pedido.AdicionarEvento(new VoucherAplicadoPedidoEvent(
            message.ClienteId,
            pedido.Id,
            voucher.Id));
        
        _pedidoRepository.AtualizarPedido(pedido);
        
        return await _pedidoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(
        IniciarPedidoCommand message,
        CancellationToken cancellationToken)
    {
        if (!ValidarComando(message)) return false;

        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
        pedido.IniciarPedido();

        var itensList = new List<Item>();
        pedido.PedidoItems.ForEach(i => itensList.Add(new Item
        {
            Id = i.ProdutoId,
            Quantidade = i.Quantidade
        }));

        var listaProdutosPedido = new ListaProdutosPedido
        {
            PedidoId = pedido.Id,
            Itens = itensList
        };
        
        pedido.AdicionarEvento(new PedidoIniciadoEvent(
            pedido.Id,
            pedido.ClienteId,
            listaProdutosPedido,
            pedido.ValorTotal,
            message.NomeCartao,
            message.NumeroCartao,
            message.ExpiracaoCartao,
            message.CvvCartao));
        
        _pedidoRepository.AtualizarPedido(pedido);

        return await _pedidoRepository.UnitOfWork.Commit();
    }

    private bool ValidarComando(Command message)
    {
        if (message.EhValido()) return true;

        foreach (var error in message.ValidationResult.Errors)
        {
            _mediator.PublicarNotificacao(
                new DomainNotification(
                    message.MessageType,
                    error.ErrorMessage));
        }

        return false;
    }
}