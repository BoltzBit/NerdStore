using NerdStore.Core.Dto;

namespace NerdStore.Core.Messages.CommonMessages.IntegrationEvents;

public class PedidoEstoqueConfirmadoEvent : IntegrationEvent
{
    public Guid PedidoId { get; private set; }
    public Guid ClienteId { get; private set; }
    public decimal Total { get; private set; }
    public ListaProdutosPedido Itens { get; private set; }
    public string NomeCartao { get; private set; }
    public string NumeroCartao { get; private set; }
    public string ExpiracaoCartao { get; private set; }
    public string CvvCartao { get; private set; }

    public PedidoEstoqueConfirmadoEvent(
        Guid pedidoId,
        Guid clienteId,
        decimal total,
        ListaProdutosPedido itens,
        string nomeCartao,
        string numeroCartao,
        string expiracaoCartao,
        string cvvCartao)
    {
        AggregateId = pedidoId;
        PedidoId = pedidoId;
        ClienteId = clienteId;
        Total = total;
        Itens = itens;
        NomeCartao = nomeCartao;
        NumeroCartao = numeroCartao;
        ExpiracaoCartao = expiracaoCartao;
        CvvCartao = cvvCartao;
    }
}