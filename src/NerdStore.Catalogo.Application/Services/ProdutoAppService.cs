using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Application.Services;

public class ProdutoAppService : IProdutoAppService
{
    private readonly IEstoqueService _estoqueService;
    private readonly IMapper _mapper;
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoAppService(
        IProdutoRepository produtoRepository,
        IEstoqueService estoqueService,
        IMapper mapper)
    {
        _produtoRepository = produtoRepository;
        _estoqueService = estoqueService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProdutoViewModel>> ObterPorCategoria(int codigo)
    {
        return _mapper
            .Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutoPorCategoria(codigo));
    }

    public async Task<ProdutoViewModel> ObterPorId(Guid id)
    {
        return _mapper
            .Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
    }

    public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
    {
        return _mapper
            .Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
    }

    public async Task<IEnumerable<CategoriaViewModel>> ObterCategorias()
    {
        return _mapper
            .Map<IEnumerable<CategoriaViewModel>>(await _produtoRepository.ObterCategorias());
    }

    public async Task AdicionarProduto(ProdutoViewModel produtoViewModel)
    {
        var produto = _mapper.Map<Produto>(produtoViewModel);

        _produtoRepository.Adicionar(produto);

        await _produtoRepository.UnitOfWork.Commit();
    }

    public async Task AtualizarProduto(ProdutoViewModel produtoViewModel)
    {
        var produto = _mapper
            .Map<Produto>(produtoViewModel);

        _produtoRepository.Atualizar(produto);

        await _produtoRepository.UnitOfWork.Commit();
    }


    public async Task<ProdutoViewModel> DebitarEstoque(Guid id, int quantidade)
    {
        var foiDebitadoEstoque = _estoqueService.DebitarEstoque(id, quantidade).Result;

        if (!foiDebitadoEstoque) throw new DomainException("Falha ao debitar o estoque");

        return _mapper
            .Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
    }

    public async Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade)
    {
        var foiRepostoEstoque = _estoqueService.ReporEstoque(id, quantidade).Result;

        if (!foiRepostoEstoque) throw new DomainException("Falha ao repor o estoque");

        return _mapper
            .Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
    }

    public void Dispose()
    {
        _produtoRepository.Dispose();
        _estoqueService.Dispose();
    }
}