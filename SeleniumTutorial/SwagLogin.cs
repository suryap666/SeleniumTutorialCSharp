using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTutorial;

public class SwagLogin
{
    private IWebDriver _driver;
    private WebDriverWait _wait;

    [SetUp]
    public void BeforeEachTest()
    {
        _driver = new ChromeDriver();
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    [TearDown]
    public void AfterEachTest()
    {
        _driver?.Quit();
        _driver?.Dispose();
    }

    [Test]
    public void ValidCredentials()
    {
        _driver.Url = "https://www.saucedemo.com/";
        
        Login(password: "secret_sauce", userName: "standard_user");


        // asset for the app logo
        var appLogo = By.XPath("//div[@class='app_logo'][text()='Swag Labs']");

        bool isDisplayed = IsDisplayed(appLogo);

        Assert.That(isDisplayed, Is.True, "When the user login successfully the we should see app logo");
    }

    [Test]
    public void InvalidCredentials()
    {
        var driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");

        Login("123", "123");

        var errorMessage = "Epic sadface: Username and password do not match any user in this service";
        var errorHeadingTag = By.XPath($"//h3[@data-test='error'][text()='{errorMessage}']");
        
        bool isDisplayed = IsDisplayed(errorHeadingTag);

        Assert.That(isDisplayed, Is.True);
    }

    private bool IsDisplayed(By byElement)
    {
        try
        {
            return _wait.Until((webDriver) => webDriver.FindElement(byElement).Displayed);
        }
        catch(Exception e)
        {
            return false;
        }
    }
    
    private bool IsEnabled(By byElement)
    {
        try
        {
            return _wait.Until((webDriver) =>  webDriver.FindElement(byElement).Enabled);
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private void Login(string userName, string password)
    {
        var userNameBy = By.CssSelector("input[data-test='username']");
        var passwordBy = By.CssSelector("input[data-test='password']");
        var submitButton = By.CssSelector("input[data-test='login-button']");

        _driver.FindElement(userNameBy).SendKeys(userName);
        _driver.FindElement(passwordBy).SendKeys(password);
        _driver.FindElement(submitButton).Click();
    }
}

// example to find selectors
// var userNameByName = By.Name("user-name");
// var userNameByCssSelector = By.CssSelector("input[class*='form_input'][placeholder='Username']]");
// var userNameByXpath = By.XPath("//input[contains(@class,'form_input')][@placeholder='Username']");
// var userNameById = By.Id("user-name");
// var userNameAndPassword = By.ClassName("form_input");
// var inputTags = By.TagName("input");
// var linkFromGoogle = By.LinkText("Xpath cheatsheet");
// var linkFromGooglePartial = By.PartialLinkText("Xpath");
