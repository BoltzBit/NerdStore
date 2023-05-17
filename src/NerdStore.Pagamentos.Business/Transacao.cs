using NerdStore.Core.DomainObjects;

namespace NerdStore.Pagamentos.Business;

public class Transacao : Entity
{
    public Guid PedidoId { get; private set; }
    public Guid PagamentoId { get; private set; }
    public decimal Total { get; private set; }
    public StatusTransacao StatusTransacao { get; private set; }
    
    public virtual Pagamento Pagamento { get; private set; }

    public Transacao(
        Guid pedidoId,
        Guid pagamentoId,
        decimal total,
        StatusTransacao statusTransacao)
    {
        PedidoId = pedidoId;
        PagamentoId = pagamentoId;
        Total = total;
        StatusTransacao = statusTransacao;
    }
}