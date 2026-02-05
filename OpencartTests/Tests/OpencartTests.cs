using NUnit.Framework;
using OpenQA.Selenium;
using OpencartTests.Helpers;
using OpencartTests.Pages;
using OpencartTests.Configuration;

namespace OpencartTests.Tests
{
    [TestFixture]
    public class OpencartTests
    {
        private IWebDriver driver;
        private DriverManager driverManager;
        private HomePage homePage;
        private TestConfiguration _config;
        
        [SetUp]
        public void Setup()
        {
            // Load configuration from appsettings.json
            _config = ConfigurationHelper.GetConfiguration();
            
            // Initialize driver with configuration
            driverManager = new DriverManager(_config.Browser.Type, _config.Browser.Headless);
            driver = driverManager.InitializeDriver();
            homePage = new HomePage(driver);
        }
        
        [Test]
        [Category("Smoke")]
        public void Test_HomePage_VerifyTitle()
        {
            // Arrange & Act
            homePage.NavigateTo(_config.BaseUrl);
            string title = homePage.GetTitle();
            
            // Assert
            Assert.That(title, Does.Contain("Your Store"));
            
            Console.WriteLine($"✓ Homepage title verified: {title}");
        }
        
        [Test]
        [Category("Smoke")]
        public void Test_NavigateToLoginPage_VerifyUrl()
        {
            // Arrange
            homePage.NavigateTo(_config.BaseUrl);
            
            // Act
            var loginPage = homePage.ClickMyAccountLogin();
            string currentUrl = loginPage.GetCurrentUrl();
            
            // Assert
            Assert.That(currentUrl, Does.Contain("account/login"));
            
            Console.WriteLine($"✓ Successfully navigated to: {currentUrl}");
        }
        
        [Test]
        [Category("Regression")]
        public void Test_Login_InvalidCredentials_ShowsError()
        {
            // Arrange
            homePage.NavigateTo(_config.BaseUrl);
            var loginPage = homePage.ClickMyAccountLogin();
            
            // Act - Using credentials from configuration
            loginPage.Login(_config.TestUsers.Invalid.Email, _config.TestUsers.Invalid.Password);
            
            // Assert
            Assert.That(loginPage.IsErrorDisplayed(), Is.True, "Error message should be displayed");
            string errorText = loginPage.GetErrorMessage();
            Assert.That(errorText, Does.Contain("Warning"));
            
            Console.WriteLine($"✓ Error message verified: {errorText}");
        }
        
        [Test]
        [Category("Regression")]
        public void Test_SearchProduct_MacBook_VerifyResults()
        {
            // Arrange
            homePage.NavigateTo(_config.BaseUrl);
            
            // Act - Using product name from configuration
            var searchResultsPage = homePage.SearchProduct(_config.TestData.SearchProducts.MacBook);
            
            // Assert
            Assert.That(searchResultsPage.IsProductDisplayed(_config.TestData.SearchProducts.MacBook), Is.True);
            
            Console.WriteLine($"✓ Search results verified for {_config.TestData.SearchProducts.MacBook}");
        }
        
        [TearDown]
        public void Teardown()
        {
            driver?.Dispose();
            driverManager?.QuitDriver();
        }
    }
}