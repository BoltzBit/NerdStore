using NerdStore.Core.Dto;

namespace NerdStore.Pagamentos.Business;

public interface IPagamentoService
{
    Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido);
}