using NUnit.Framework;
using OpenQA.Selenium;
using OpencartTests.Helpers;
using OpencartTests.Pages;

namespace OpencartTests.Tests
{
    [TestFixture]
    public class OpencartTests
    {
        private IWebDriver driver;
        private DriverManager driverManager;
        private HomePage homePage;
        private const string BASE_URL = "https://naveenautomationlabs.com/opencart";
        
        [SetUp]
        public void Setup()
        {
            driverManager = new DriverManager();
            driver = driverManager.InitializeDriver();
            homePage = new HomePage(driver);
        }
        
        [Test]
        [Category("Smoke")]
        public void Test_HomePage_VerifyTitle()
        {
            // Arrange & Act
            homePage.NavigateTo(BASE_URL);
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
            homePage.NavigateTo(BASE_URL);
            
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
            homePage.NavigateTo(BASE_URL);
            var loginPage = homePage.ClickMyAccountLogin();
            
            // Act
            loginPage.Login("invalid@test.com", "wrongpassword");
            
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
            homePage.NavigateTo(BASE_URL);
            
            // Act
            var searchResultsPage = homePage.SearchProduct("MacBook");
            
            // Assert
            Assert.That(searchResultsPage.IsProductDisplayed("MacBook"), Is.True);
            
            Console.WriteLine("✓ Search results verified for MacBook");
        }
        
        [TearDown]
        public void Teardown()
        {
            driver?.Dispose();
            driverManager?.QuitDriver();
        }
    }
}