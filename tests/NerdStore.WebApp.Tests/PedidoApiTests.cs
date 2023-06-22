using System.Net.Http.Json;
using NerdStore.WebApp.MVC.Models;
using NerdStore.WebApp.Tests.Config;
using NerdStore.WebApp.Tests.Order;
using Xunit;

namespace NerdStore.WebApp.Tests;

[TestCaseOrderer("NerdStore.WebApp.Tests.PriorityOrderer", "NerdStore.WebApp.Tests")]
[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class PedidoApiTests
{
    private readonly IntegrationTestsFixture<Program> _testsFixture;

    public PedidoApiTests(IntegrationTestsFixture<Program> testsFixture)
    {
        _testsFixture = testsFixture;
    }

    [Fact(DisplayName = "Adicionar item em novo pedido"), TestPriority(1)]
    [Trait("Categoria", "Integração API - Pedido")]
    public async Task AdicionarItem_NovoPedido_DeveRetornarComSucesso()
    {
        //Arrange
        var itemInfo = new ItemViewModel
        {
            Id = new Guid("047fd48d-8eec-4a2e-c1b3-08db46bd52bc"),
            Quantidade = 2
        };

        await _testsFixture.RealizarLoginApi();
        _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);
        
        //Act
        var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/carrinho", itemInfo);
        
        //Assert
        postResponse.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "Remover item em pedido existente"), TestPriority(2)]
    [Trait("Categoria", "Integração API - Pedido")]
    public async Task RemoverItem_PedidoExistente_DeveRetornarComSucess()
    {
        //Arrange
        var produtoId = new Guid("047fd48d-8eec-4a2e-c1b3-08db46bd52bc");
        await _testsFixture.RealizarLoginApi();
        _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);
        
        //Act
        var deleteResponse = await _testsFixture.Client.DeleteAsync($"api/carrinho/{produtoId}");
        
        
        //Assert
        deleteResponse.EnsureSuccessStatusCode();
    }
}