using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using OpenQA.Selenium;
using OpencartTests.Helpers;
using OpencartTests.Pages;
using OpencartTests.Configuration;
using static OpencartTests.Helpers.TestData;

namespace OpencartTests.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("UI Tests")]
    public class OpencartTests
    {
        private IWebDriver driver;
        private DriverManager driverManager;
        private HomePage homePage;
        
        [SetUp]
        public void Setup()
        {
            // Simple config - just 3 values from appsettings.json
            driverManager = new DriverManager(Config.BrowserType, Config.BrowserHeadless);
            driver = driverManager.InitializeDriver();
            homePage = new HomePage(driver);
        }
        
        [Test]
        [Category("Smoke")]
        public void Test_HomePage_VerifyTitle()
        {
            // Arrange & Act
            homePage.NavigateTo(Config.BaseUrl);
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
            homePage.NavigateTo(Config.BaseUrl);
            
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
            homePage.NavigateTo(Config.BaseUrl);
            var loginPage = homePage.ClickMyAccountLogin();
            
            // Act - Using simple test data (no complex config!)
            loginPage.Login(Users.Invalid.Email, Users.Invalid.Password);
            
            // Assert
            Assert.That(loginPage.IsErrorDisplayed(), Is.True, "Error message should be displayed");
            string errorText = loginPage.GetErrorMessage();
            Assert.That(errorText, Does.Contain(ErrorMessages.InvalidLogin));
            
            Console.WriteLine($"✓ Error message verified: {errorText}");
        }
        
        [Test]
        [Category("Regression")]
        public void Test_SearchProduct_MacBook_VerifyResults()
        {
            // Arrange
            homePage.NavigateTo(Config.BaseUrl);
            
            // Act - Using simple test data (no complex config!)
            var searchResultsPage = homePage.SearchProduct(Products.MacBook);
            
            // Assert
            Assert.That(searchResultsPage.IsProductDisplayed(Products.MacBook), Is.True);
            
            Console.WriteLine($"✓ Search results verified for {Products.MacBook}");
        }
        
        [TearDown]
        public void Teardown()
        {
            driver?.Dispose();
            driverManager?.QuitDriver();
        }
    }
}