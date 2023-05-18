using NerdStore.Core.DomainObjects;

namespace NerdStore.Pagamentos.Business;

public class Pagamento : Entity, IAggregateRoot
{
    public Guid PedidoId { get; private set; }
    public string Status { get; set; }
    public decimal Valor { get; private set; }
    
    public string NomeCartao { get; private set; }
    public string NumeroCartao { get; private set; }
    public string ExpiracaoCartao { get; private set; }
    public string CvvCartao { get; private set; }
    
    public virtual Transacao Transacao { get; private set; }

    public Pagamento(
        Guid pedidoId,
        decimal valor,
        string nomeCartao,
        string numeroCartao,
        string expiracaoCartao,
        string cvvCartao)
    {
        PedidoId = pedidoId;
        Valor = valor;
        NomeCartao = nomeCartao;
        NumeroCartao = numeroCartao;
        ExpiracaoCartao = expiracaoCartao;
        CvvCartao = cvvCartao;
    }
}