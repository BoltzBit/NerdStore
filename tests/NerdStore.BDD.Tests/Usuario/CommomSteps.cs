using NerdStore.BDD.Tests.Config;
using Xunit;

namespace NerdStore.BDD.Tests.Usuario;

[Binding]
[CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
public class CommomSteps
{
    private readonly CadastroUsuarioTela _cadastroUsuarioTela;
    private readonly AutomacaoWebTestsFixture _testsFixture;

    public CommomSteps(AutomacaoWebTestsFixture testsFixture)
    {
        _testsFixture = testsFixture;
        _cadastroUsuarioTela = new CadastroUsuarioTela(testsFixture.BrowserHelper);
    }

    [Given(@"Que o visitante está acessando o site da loja")]
    public void DadoQueOVisitanteEstaAcessandoOSiteDaLoja()
    {
        //Act
        _cadastroUsuarioTela.AcessarSiteLoja();
        
        //Assert
        Assert.Contains(_testsFixture.Configuration.DomainUrl, 
            _cadastroUsuarioTela.ObterUrl());
    }

    [Then(@"Ele será redirecionado para a vitrine")]
    public void EntaoEleSeraRedirecionadoParaAVitrine()
    {
        //Assert
        Assert.Equal(_testsFixture.Configuration.VitriniUrl, _cadastroUsuarioTela.ObterUrl());
    }

    [Then(@"Uma saudação com seu e-mail será exibida no menu superior")]
    public void EntaoUmaSaudacaoComSeuE_MailSeraExibidaNoMenuSuperior()
    {
        //Assert
        Assert.True(_cadastroUsuarioTela.ValidarSaudacaoUsuarioLogado(_testsFixture.Usuario));
    }
}