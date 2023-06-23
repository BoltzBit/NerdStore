using Microsoft.Extensions.Configuration;

namespace NerdStore.BDD.Tests.Config;

public class ConfigurationHelper
{
    private readonly IConfiguration _config;

    public ConfigurationHelper() 
        => _config = new ConfigurationManager()
            .AddJsonFile("appsettings.json")
            .Build();

    public string DomainUrl => _config.GetSection("DomainUrl").Value;
    public string VitriniUrl => _config.GetSection("VitrineUrl").Value;
    public string ProdutoUrl => $"{DomainUrl}{_config.GetSection("CarrinhoUrl").Value}";
    public string RegisterUrl => $"{DomainUrl}{_config.GetSection("RegisterUrl").Value}";
    public string LoginUrl => $"{DomainUrl}{_config.GetSection("LoginUrl").Value}";
    public string WebDrivers => _config.GetSection("WebDrivers").Value;
    public string FolderPath => Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
    public string FolderPicture => $"{FolderPath}{_config.GetSection("FolderPicture").Value}";
}