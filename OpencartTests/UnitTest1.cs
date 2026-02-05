using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace OpencartTests
{
    [TestFixture]
    public class OpencartLoginTests
    {
        private IWebDriver driver;
        private const string BASE_URL = "https://naveenautomationlabs.com/opencart";
        
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }
        
        [Test]
        public void Test_OpenHomePage_VerifyTitle()
        {
            // Navigate to homepage
            driver.Navigate().GoToUrl(BASE_URL);
            
            // Get page title
            string title = driver.Title;
            
            // Verify title contains "Your Store"
            Assert.That(title, Does.Contain("Your Store"));
            
            Console.WriteLine($"✓ Test passed! Page title: {title}");
        }

    [Test]
public void Test_NavigateToLoginPage_VerifyUrl()
{
    // Navigate to homepage
    driver.Navigate().GoToUrl(BASE_URL);
    
    // Click "My Account" dropdown
    var myAccountDropdown = driver.FindElement(By.LinkText("My Account"));
    myAccountDropdown.Click();
    
    // Click "Login"
    var loginLink = driver.FindElement(By.LinkText("Login"));
    loginLink.Click();
    
    // Verify we're on login page
    string currentUrl = driver.Url;
    Assert.That(currentUrl, Does.Contain("account/login"));
    
    Console.WriteLine($"✓ Successfully navigated to login page: {currentUrl}");
}

    [Test]
    public void Test_SearchProduct_VerifyResults()
    {
        // Navigate to homepage
        driver.Navigate().GoToUrl(BASE_URL);
        
        // Find search box and enter product
        var searchBox = driver.FindElement(By.Name("search"));
        searchBox.SendKeys("MacBook");
        
        // Click search button
        var searchButton = driver.FindElement(By.CssSelector("button.btn-default"));
        searchButton.Click();
        
        // Verify search results page loaded
        string pageSource = driver.PageSource;
        Assert.That(pageSource, Does.Contain("MacBook"));
        
        Console.WriteLine("✓ Search results displayed for MacBook");
    }
        
        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}