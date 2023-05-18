using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Data.Repository;

public class PedidoRepository : IPedidoRepository
{
    private readonly VendasDbContext _context;

    public PedidoRepository(VendasDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<Pedido?> ObterPorId(Guid id)
    {
        return await _context
            .Pedidos
            .FindAsync(id);
    }

    public async Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId)
    {
        return await _context
            .Pedidos
            .AsNoTracking()
            .Where(p => p.ClienteId == clienteId)
            .ToListAsync();
    }

    public async Task<Pedido?> ObterPedidoRascunhoPorClienteId(Guid clienteId)
    {
        var pedido = await _context
            .Pedidos
            .FirstOrDefaultAsync(p => p.ClienteId == clienteId &&
                                      p.PedidoStatus == PedidoStatus.Rascunho);

        if (pedido is null) return pedido;
        
        await _context
            .Entry(pedido)
            .Collection(p => p.PedidoItems)
            .LoadAsync();

        if (pedido.VoucherId is not null)
        {
            await _context
                .Entry(pedido)
                .Reference(p => p.Voucher)
                .LoadAsync();
        }

        return pedido;
    }

    public void Adicionar(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
    }

    public void AtualizarPedido(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
    }

    public async Task<PedidoItem?> ObterItemPorPedido(Guid pedidoId, Guid produtoId)
    {
        return await _context
            .PedidoItems
            .FirstOrDefaultAsync(p => p.ProdutoId == produtoId &&
                                      p.PedidoId == pedidoId);
    }

    public void AdicionarItem(PedidoItem pedidoItem)
    {
        _context.PedidoItems.Add(pedidoItem);
    }

    public void AtualizarItem(PedidoItem pedidoItem)
    {
        _context.PedidoItems.Update(pedidoItem);
    }

    public void RemoverItem(PedidoItem pedidoItem)
    {
        _context.PedidoItems.Remove(pedidoItem);
    }

    public async Task<Voucher?> ObterVoucherPorCodigo(string codigo)
    {
        return await _context
            .Vouchers
            .FirstOrDefaultAsync(v => v.Codigo == codigo);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}