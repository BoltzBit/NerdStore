﻿using NerdStore.WebApp.Tests.Config;
using Xunit;

namespace NerdSore.WebApp.Tests;

[Collection(nameof(IntegrationWebTestsFixtureCollection))]
public class UsuarioTests
{
    private readonly IntegrationTestsFixture<Program> _testsFixture;

    public UsuarioTests(IntegrationTestsFixture<Program> testsFixture)
    {
        _testsFixture = testsFixture;
    }

    [Fact(DisplayName = "Realizar cadastro com sucesso")]
    [Trait("Categoria", "Integração Web - Usuário")]
    public async Task Usuario_RealizarCadastro_DeveExecutarComSucesso()
    {
        // Arrange
        var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
        initialResponse.EnsureSuccessStatusCode();

        //Asp Net MVC somente retorna resposta para token que foi gerado na requisicao
        var antiForgeryToken = _testsFixture
            .ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());
        
        _testsFixture.GerarUserSenha();

        var formData = new Dictionary<string, string>
        {
            { _testsFixture.AntiForgeryFieldName, antiForgeryToken },
            { "Input.Email", _testsFixture.UsuarioEmail },
            { "Input.Password", _testsFixture.UsuarioSenha },
            { "Input.ConfirmPassword", _testsFixture.UsuarioSenha }
        };

        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Register")
        {
            Content = new FormUrlEncodedContent(formData)
        };

        // Act
        var postResponse = await _testsFixture.Client.SendAsync(postRequest);
        
        // Assert
        var responseString = await postResponse.Content.ReadAsStringAsync();
        postResponse.EnsureSuccessStatusCode();

        Assert.Contains("Register confirmation", responseString);
    }

    [Fact(DisplayName = "Realizar cadastro senha fraca")]
    [Trait("Categoria", "Integração Web - Usuário")]
    public async Task Usuario_RealizarCadastroComSenhaFraca_DeveRetornarMensagemDeErro()
    {
        //Arrange
        var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
        initialResponse.EnsureSuccessStatusCode();

        var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());
        
        _testsFixture.GerarUserSenha();
        const string senhaFraca = "123456";

        var formData = new Dictionary<string, string>
        {
            {_testsFixture.AntiForgeryFieldName, antiForgeryToken},
            {"Input.Email", _testsFixture.UsuarioEmail},
            {"Input.Password", senhaFraca },
            {"Input.ConfirmPassword", senhaFraca}
        };

        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Register")
        {
            Content = new FormUrlEncodedContent(formData)
        };
        
        //Act
        var postResponse = await _testsFixture.Client.SendAsync(postRequest);
        
        //Assert
        var responseString = await postResponse.Content.ReadAsStringAsync();

        postResponse.EnsureSuccessStatusCode();
        Assert.Contains("Passwords must have at least one non alphanumeric character.", responseString);
        Assert.Contains("Passwords must have at least one lowercase (&#x27;a&#x27;-&#x27;z&#x27;).", responseString);
        Assert.Contains("Passwords must have at least one uppercase (&#x27;A&#x27;-&#x27;Z&#x27;).", responseString);
    }

    [Fact(DisplayName = "Realizar login com sucesso")]
    [Trait("Categoria", "Integração Web - Usuário")]
    public async Task Usuario_RealizarLogin_DeveExecutarComSucesso()
    {
        //Arrange
        var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Login");
        initialResponse.EnsureSuccessStatusCode();

        var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

        var formData = new Dictionary<string, string>
        {
            {_testsFixture.AntiForgeryFieldName, antiForgeryToken},
            {"Input.Email", "teste@teste.com"},
            {"Input.Password", "teste@teste2A"}
        };

        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Login")
        {
            Content = new FormUrlEncodedContent(formData)
        };
        
        //Act
        var postResponse = await _testsFixture.Client.SendAsync(postRequest);
        
        //Assert
        var responseString = await postResponse.Content.ReadAsStringAsync();

        postResponse.EnsureSuccessStatusCode();
        Assert.Contains($"Hello {_testsFixture.UsuarioEmail}!", responseString);
    }
}