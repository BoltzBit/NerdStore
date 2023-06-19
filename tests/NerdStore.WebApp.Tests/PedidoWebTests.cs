using AngleSharp.Html.Parser;
using NerdStore.WebApp.Tests.Config;
using Xunit;

namespace NerdStore.WebApp.Tests;

[Collection(nameof(IntegrationWebTestsFixtureCollection))]
public class PedidoWebTests
{
    private readonly IntegrationTestsFixture<Program> _testsFixture;

    public PedidoWebTests(IntegrationTestsFixture<Program> testsFixture)
    {
        _testsFixture = testsFixture;
    }

    [Fact(DisplayName = "Adicionar item em novo pedido")]
    [Trait("Categoria", "Integração Web - Pedido")]
    public async Task AdicionarItem_NovoPedido_DeveAtualizarValorTotal()
    {
        //Arrange
        var produtoId = new Guid("047fd48d-8eec-4a2e-c1b3-08db46bd52bc");
        const int quantidade = 2;

        var initialResponse = await _testsFixture.Client.GetAsync($"/produto-detalhe/{produtoId}");
        initialResponse.EnsureSuccessStatusCode();

        var formData = new Dictionary<string, string>
        {
            {"Id", $"{produtoId}"},
            {"quantidade", $"{quantidade}"}
        };

        await _testsFixture.RealizarLoginWeb();

        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/meu-carrinho")
        {
            Content = new FormUrlEncodedContent(formData)
        };
        
        //Act
        var postResponse = await _testsFixture.Client.SendAsync(postRequest);
        
        //Assert
        postResponse.EnsureSuccessStatusCode();

        var html = new HtmlParser()
            .ParseDocumentAsync(await postResponse.Content.ReadAsStringAsync())
            .Result
            .All;

        var formQuantidade = html?
            .FirstOrDefault(e => e.Id == "quantidade")?
            .GetAttribute("value")?
            .ApenasNumeros();
        
        var valorUnitario = html?
            .FirstOrDefault(e => e.Id == "valorUnitario")?
            .TextContent.Split(".")[0]?
            .ApenasNumeros();

        var valorTotal = html?
            .FirstOrDefault(e => e.Id == "valorTotal")?
            .TextContent.Split(".")[0]?
            .ApenasNumeros();
        
        Assert.Equal(valorTotal, valorUnitario * formQuantidade);
    }
}