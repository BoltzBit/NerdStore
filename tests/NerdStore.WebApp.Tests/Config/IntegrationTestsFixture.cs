using System.Text.RegularExpressions;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace NerdStore.WebApp.Tests.Config;
[CollectionDefinition(nameof(IntegrationWebTestsFixtureCollection))]
public class IntegrationWebTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>>{ }
public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
{
    public string AntiForgeryFieldName = "__RequestVerificationToken";

    public string UsuarioEmail;
    public string UsuarioSenha;
    
    public readonly LojaAppFactory<TProgram> Factory;
    public HttpClient Client;

    public IntegrationTestsFixture()
    {
        var options = new WebApplicationFactoryClientOptions();
        
        Factory = new LojaAppFactory<TProgram>();
        Client = Factory.CreateClient(options);
    }

    public string ObterAntiForgeryToken(string htmlBody)
    {
        var requestVerificationTokenMatch = Regex
            .Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");

        if (requestVerificationTokenMatch.Success)
        {
            return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
        }

        throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' não encontrado no HTML", nameof(htmlBody));
    }

    public async Task RealizarLoginWeb()
    {
        var initialResponse = await Client.GetAsync("/Identity/Account/Login");
        initialResponse.EnsureSuccessStatusCode();

        var antiForgeryToken = ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

        var formData = new Dictionary<string, string>
        {
            {AntiForgeryFieldName, antiForgeryToken},
            {"Input.Email", "teste@teste.com"},
            {"Input.Password", "teste@teste2A"}
        };

        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Login")
        {
            Content = new FormUrlEncodedContent(formData)
        };

        await Client.SendAsync(postRequest);
    }

    public void GerarUserSenha()
    {
        var faker = new Faker("pt_BR");
        UsuarioEmail = faker.Internet.Email().ToLower();
        UsuarioSenha = faker.Internet.Password(8, false, "", "@1Ab_");
    }

    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}