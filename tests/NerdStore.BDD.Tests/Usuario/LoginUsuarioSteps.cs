using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.Usuario;

[Binding]
public sealed class LoginUsuarioSteps
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;

    public LoginUsuarioSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }
}