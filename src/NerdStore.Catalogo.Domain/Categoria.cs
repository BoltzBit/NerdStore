﻿using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain;

public class Categoria : Entity
{
    public Categoria(
        string nome,
        int codigo)
    {
        Nome = nome;
        Codigo = codigo;
        
        Validar();
    }

    public string Nome { get; private set; }
    public int Codigo { get; private set; }

    public IReadOnlyCollection<Produto> Produtos { get; private set; }
    
    protected Categoria()
    {
    }

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