using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.Usuario;

[Binding]
public sealed class CadastroUsuarioSteps
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;

    public CadastroUsuarioSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }
}