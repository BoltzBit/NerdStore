using NerdStore.Core.Dto;

namespace NerdStore.Catalogo.Domain;

public interface IEstoqueService : IDisposable
{
    Task<bool> DebitarEstoque(Guid produtoId, int quantidade);
    Task<bool> ReporEstoque(Guid produtoId, int quantidade);
    Task<bool> ReporListaProdutosPedido(ListaProdutosPedido listaProdutosPedido);
    Task<bool> DebitarListaProdutosPedido(ListaProdutosPedido lista);
}