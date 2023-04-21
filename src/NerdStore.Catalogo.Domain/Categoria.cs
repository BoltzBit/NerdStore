using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain;

public class Categoria : Entity
{
    protected Categoria()
    {
    }

    public Categoria(
        string nome,
        int codigo)
    {
        Nome = nome;
        Codigo = codigo;
    }

    public string Nome { get; }
    public int Codigo { get; }

    public IReadOnlyCollection<Produto> Produtos { get; private set; }

    public override string ToString()
    {
        return $"{Nome} - {Codigo}";
    }

    public void Validar()
    {
        Validacoes.ValidarSeVazio(Nome, "O campo Nome da categoria não pode estar vazio");
        Validacoes.ValidarSeIgual(Codigo, 0, "O campo Codigo não pode ser 0");
    }
}