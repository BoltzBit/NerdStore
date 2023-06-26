namespace NerdStore.BDD.Tests.Pedido;

[Binding]
public sealed class AdicionarItemSteps
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;

    public AdicionarItemSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"Que um produto esteja na vitrine")]
    public void GivenQueUmProdutoEstejaNaVitrine()
    {
        ScenarioContext.StepIsPending();
    }

    [Given(@"Esteja disponivel no estoque")]
    public void GivenEstejaDisponivelNoEstoque()
    {
        ScenarioContext.StepIsPending();
    }

    [Given(@"O usuario esteja logado")]
    public void GivenOUsuarioEstejaLogado()
    {
        ScenarioContext.StepIsPending();
    }

    [When(@"O usuário adicionar uma unidade ao carrinho")]
    public void WhenOUsuarioAdicionarUmaUnidadeAoCarrinho()
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"O usuário será redirecionado ao resumo da compra")]
    public void ThenOUsuarioSeraRedirecionadoAoResumoDaCompra()
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"O valor total do pedido será exatamente o valor do item adicionado")]
    public void ThenOValorTotalDoPedidoSeraExatamenteOValorDoItemAdicionado()
    {
        ScenarioContext.StepIsPending();
    }

    [When(@"O usuário adicionar um item acima da quantidade máxima permitida")]
    public void WhenOUsuarioAdicionarUmItemAcimaDaQuantidadeMaximaPermitida()
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"Receberá uma mensagem de erro mencionando que foi ultrapassada a quantidade limite")]
    public void ThenReceberaUmaMensagemDeErroMencionandoQueFoiUltrapassadaAQuantidadeLimite()
    {
        ScenarioContext.StepIsPending();
    }

    [Given(@"O mesmo produto já tenha sido adicionado ao carrinho anteriormente")]
    public void GivenOMesmoProdutoJaTenhaSidoAdicionadoAoCarrinhoAnteriormente()
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"A quantidade de itens daquele produto terá sido acrescida em uma unidade a mais")]
    public void ThenAQuantidadeDeItensDaqueleProdutoTeraSidoAcrescidaEmUmaUnidadeAMais()
    {
        ScenarioContext.StepIsPending();
    }

    [When(@"O usuário adicionar a quantidade máxima permitida ao carrinho")]
    public void WhenOUsuarioAdicionarAQuantidadeMaximaPermitidaAoCarrinho()
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"O valor total do pedido será a multiplicação da quantidade de itens pelo valor unitario")]
    public void ThenOValorTotalDoPedidoSeraAMultiplicacaoDaQuantidadeDeItensPeloValorUnitario()
    {
        ScenarioContext.StepIsPending();
    }
}