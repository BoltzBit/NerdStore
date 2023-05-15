using FluentValidation;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands;

public class AtualizarItemPedidoCommand : Command
{
    public Guid ClienteId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public int Quantidade { get; private set; }

    public AtualizarItemPedidoCommand(
        Guid clienteId,
        Guid produtoId,
        int quantidade)
    {
        ClienteId = clienteId;
        ProdutoId = produtoId;
        Quantidade = quantidade;
    }

    public override bool EhValido()
    {
        ValidationResult =  new AtualizarItemPedidoValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class AtualizarItemPedidoValidation : AbstractValidator<AtualizarItemPedidoCommand>
{
    public AtualizarItemPedidoValidation()
    {
        RuleFor(a => a.ClienteId)
            .NotEqual(Guid.Empty)
            .WithMessage("Id do cliente inválido");

        RuleFor(a => a.ProdutoId)
            .NotEqual(Guid.Empty)
            .WithMessage("Id do produto inválido");

        RuleFor(a => a.Quantidade)
            .GreaterThan(0)
            .WithMessage("A quantidade miníma de um item é 1");

        RuleFor(a => a.Quantidade)
            .LessThan(15)
            .WithMessage("A quantidade máxima de um item é 15");
    }
}