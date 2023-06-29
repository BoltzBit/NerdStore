using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NerdStore.BDD.Tests.Config;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Usuario;

[Binding]
[CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
public class LoginUsuarioSteps
{
    private readonly LoginUsuarioTela _loginUsuarioTela;
    private readonly AutomacaoWebTestsFixture _testsFixture;

    public LoginUsuarioSteps(AutomacaoWebTestsFixture testsFixture)
    {
        _testsFixture = testsFixture;
        _loginUsuarioTela = new LoginUsuarioTela(_testsFixture.BrowserHelper);
    }

    [When(@"Ele clicar em login")]
    public void WhenEleClicarEmLogin()
    {
        //Act
        _loginUsuarioTela.ClicarNoLinkLogin();
        
        //Assert
        Assert.Contains(
            _testsFixture.Configuration.LoginUrl,
            _loginUsuarioTela.ObterUrl());
    }

    [When(@"Preencher os dados do formulario de login")]
    public void WhenPreencherOsDadosDoFormularioDeLogin(Table table)
    {
        //Arrange
        var usuario = new Usuario
        {
            Email = "teste@teste.com",
            Senha = "teste@teste2A"
        };

        _testsFixture.Usuario = usuario;
        
        //Act
        _loginUsuarioTela.PreencherFormularioLogin(usuario);
        
        //Assert
        Assert.True(_loginUsuarioTela.ValidarPreenchimentoFormularioLogin(usuario));
    }

    [When(@"Clicar no botão login")]
    public void WhenClicarNoBotaoLogin()
    {
        _loginUsuarioTela.ClicarNoBotaoLogin();
    }
}