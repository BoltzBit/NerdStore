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
public class CadastroUsuarioSteps
{
    private readonly CadastroUsuarioTela _cadastroUsuarioTela;
    private readonly AutomacaoWebTestsFixture _testsFixture;

    public CadastroUsuarioSteps(
        AutomacaoWebTestsFixture testsFixture)
    {
        _testsFixture = testsFixture;
        _cadastroUsuarioTela = new CadastroUsuarioTela(_testsFixture.BrowserHelper);
    }

    [When(@"Ele clicar em registrar")]
    public void WhenEleClicarEmRegistrar()
    {
        //Act
        _cadastroUsuarioTela.ClicarNoBotaoRegistrar();
        
        //Assert
        Assert.Contains(
            _testsFixture.Configuration.RegisterUrl, 
            _cadastroUsuarioTela.ObterUrl());
    }

    [When(@"Preencher os dados do formulario")]
    public void WhenPreencherOsDadosDoFormulario(Table table)
    {
        //Arrange
        _testsFixture.GerarDadosUsuario();
        var usuario = _testsFixture.Usuario;
        
        //Act
        _cadastroUsuarioTela.PreencherFormularioRegistro(usuario);
        
        //Assert
        Assert.True(_cadastroUsuarioTela.ValidarPreenchimentoFormularioRegistro(usuario));
    }

    [When(@"Clicar no botão registrar")]
    public void WhenClicarNoBotaoRegistrar()
    {
        _cadastroUsuarioTela.ClicarNoBotaoRegistrar();
    }

    [When(@"Preencher os dados do formulario com uma senha sem maiusculas")]
    public void WhenPreencherOsDadosDoFormularioComUmaSenhaSemMaiusculas(Table table)
    {
        //Arrange
        _testsFixture.GerarDadosUsuario();
        var usuario = _testsFixture.Usuario;
        usuario.Senha = "teste@123";

        //Act
        _cadastroUsuarioTela.PreencherFormularioRegistro(usuario);
        
        //Assert
        Assert.True(_cadastroUsuarioTela.ValidarPreenchimentoFormularioRegistro(usuario));
    }
    
    [When(@"Preencher os dados do formulario com uma senha sem caractere especial")]
    public void WhenPreencherOsDadosDoFormularioComUmaSenhaSemCaractereEspecial(Table table)
    {
        //Arrange
        _testsFixture.GerarDadosUsuario();
        var usuario = _testsFixture.Usuario;
        usuario.Senha = "Teste123";

        //Act
        _cadastroUsuarioTela.PreencherFormularioRegistro(usuario);

        //Assert
        Assert.True(_cadastroUsuarioTela
            .ValidarPreenchimentoFormularioRegistro(usuario));
    }

    [Then(@"Ele receberá uma mensagem de erro que a senha precisa conter uma letra maiuscula")]
    public void ThenEleReceberaUmaMensagemDeErroQueASenhaPrecisaConterUmaLetraMaiuscula()
    {
        Assert.True(_cadastroUsuarioTela
            .ValidarMensagemDeErroFormulario("Passwords must have at least one uppercase ('A'-'Z')"));
    }

    [Then(@"Ele receberá uma mensagem de erro que a senha precisa conter um caractere especial")]
    public void ThenEleReceberaUmaMensagemDeErroQueASenhaPrecisaConterUmCaractereEspecial()
    {
        Assert.True(_cadastroUsuarioTela
            .ValidarMensagemDeErroFormulario("Passwords must have at least one non alphanumeric character"));
    }
}