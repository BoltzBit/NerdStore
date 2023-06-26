using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace NerdStore.BDD.Tests.Config;

public class SeleniumHelper : IDisposable
{
    public readonly ConfigurationHelper Configuration;
    public IWebDriver WebDriver;
    public WebDriverWait Wait;

    public SeleniumHelper(
        Browser browser,
        ConfigurationHelper configuration,
        bool headless = true)
    {
        Configuration = configuration;
        WebDriver = WebDriverFactory.CreateWebDriver(browser, Configuration.WebDrivers, headless);
        WebDriver.Manage().Window.Maximize();
        Wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30));
    }

    public string ObterUrl()
    {
        return WebDriver.Url;
    }

    public void IrParaUrl(string url)
    {
        WebDriver.Navigate().GoToUrl(url);
    }

    public bool ValidarConteudoUrl(string conteudo)
    {
        return Wait.Until(ExpectedConditions.UrlContains(conteudo));
    }

    public void ClicarLinkPorTexto(string linkText)
    {
        var link = Wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText(linkText)));
        link.Click();
    }

    public void ClicarBotaoPorId(string botaoId)
    {
        var botao = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(botaoId)));
        botao.Click();
    }

    public void ClicarPorXPath(string xPath)
    {
        var elemento = Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));
        elemento.Click();
    }

    public IWebElement ObterElementoPorClasse(string classeCss)
    {
        return Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(classeCss)));
    }

    public IWebElement ObterElementoPorXPath(string xPath)
    {
        return Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));
    }

    public void PreencherTextBoxPorId(string idCampo, string valorCampo)
    {
        var campo = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(idCampo)));
        campo.SendKeys(valorCampo);
    }

    public void PreencherDropDownPorId(string idCampo, string valorCampo)
    {
        var campo = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(idCampo)));
        var selectElement = new SelectElement(campo);
        selectElement.SelectByValue(valorCampo);
    }

    public string ObterTextElementoPorClasseCss(string classeName)
    {
        return Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(classeName))).Text;
    }

    public string ObterTextoElementoPorId(string id)
    {
        return Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id))).Text;
    }

    public string ObterValorTextBoxPorId(string id)
    {
        return Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)))
            .GetAttribute("value");
    }

    public IEnumerable<IWebElement> ObterListaPorClasse(string classeName)
    {
        return Wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName(classeName)));
    }

    public bool ValidarSeElementoExistePorId(string id)
    {
        return ElementoExistente(By.Id(id));
    }

    private bool ElementoExistente(By by)
    {
        try
        {
            WebDriver.FindElement(by);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public void VoltarNavegacao(int vezes = 1)
    {
        for (var i = 0; i < vezes; i++)
        {
            WebDriver.Navigate().Back();
        }
    }

    public void ObterScreenShot(string nome)
    {
        SalvarScreenShot(WebDriver.TakeScreenshot(), string.Format("{0}" + nome + ".png", DateTime.Now.ToFileTime()));
    }

    private void SalvarScreenShot(Screenshot screenshot, string fileName)
    {
        screenshot.SaveAsFile($"{Configuration.FolderPicture}{fileName}", ScreenshotImageFormat.Png);
    }

    public void Dispose()
    {
        WebDriver.Quit();
        WebDriver.Dispose();
    }
}