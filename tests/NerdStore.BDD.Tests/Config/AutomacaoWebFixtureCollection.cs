using Xunit;

namespace NerdStore.BDD.Tests.Config;

[CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
public class AutomacaoWebFixtureCollection : ICollectionFixture<AutomacaoWebTestsCollection> {}

public class AutomacaoWebTestsCollection
{
    public readonly ConfigurationHelper Configuration;
    public SeleniumHelper BrowserHelper;

    public AutomacaoWebTestsCollection()
    {
        Configuration = new ConfigurationHelper();
        BrowserHelper = new SeleniumHelper(Browser.Chrome, Configuration);
    }
}