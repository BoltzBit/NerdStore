using NerdStore.Vendas.Application.Queries.ViewModels;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Queries;

public class PedidoQueries : IPedidoQueries
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoQueries(
        IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CarrinhoViewModel?> ObterCarrinhoCliente(Guid clienteId)
    {
        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(clienteId);

        if (pedido is null) return null;

        var carrinho = new CarrinhoViewModel
        {
            ClienteId = pedido.ClienteId,
            PedidoId = pedido.Id,
            ValorTotal = pedido.ValorTotal,
            ValorDesconto = pedido.Desconto,
            SubTotal = pedido.Desconto + pedido.ValorTotal
        };

        if (pedido.VoucherId is not null)
        {
            carrinho.VoucherCodigo = pedido.Voucher.Codigo;
        }

        foreach (var pedidoItem in pedido.PedidoItems)
        {
            carrinho.Items.Add(new CarrinhoItemViewModel
            {
                ProdutoId = pedidoItem.ProdutoId,
                ProdutoNome = pedidoItem.ProdutoNome,
                Quantidade = pedidoItem.Quantidade,
                ValorUnitario = pedidoItem.ValorUnitario,
                ValorTotal = pedidoItem.ValorUnitario + pedidoItem.Quantidade
            });
        }

        return carrinho;
    }

    public async Task<IEnumerable<PedidoViewModel>> ObterPedidosCliente(Guid clienteId)
    {
        var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);
        
        var pedidosViewModel = new List<PedidoViewModel>();

        if (!pedidos.Any()) return pedidosViewModel;

        pedidos = pedidos
            .Where(p => p.PedidoStatus == PedidoStatus.Pago ||
                        p.PedidoStatus == PedidoStatus.Cancelado)
            .OrderByDescending(p => p.Codigo);

        foreach (var pedido in pedidos)
        {
            pedidosViewModel.Add(
                new PedidoViewModel
                {
                    Codigo = pedido.Codigo,
                    PedidoStatus = (int)pedido.PedidoStatus,
                    ValorTotal = pedido.ValorTotal,
                    DataCadastro = pedido.DataCadastro
                });
        }

        return pedidosViewModel;
    }
}