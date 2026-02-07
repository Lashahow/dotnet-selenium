using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using OpenQA.Selenium;
using OpencartTests.Helpers;
using OpencartTests.Pages;
using OpencartTests.Configuration;

namespace OpencartTests.Tests.UITests
{
    /// <summary>
    /// Tests for user registration functionality
    /// Uses dynamic test data (Faker/Bogus) to generate unique data for each run
    /// </summary>
    [TestFixture]
    [Category("Regression")]
    [AllureNUnit]
    [AllureSuite("Registration Tests")]
    public class RegistrationTests
    {
        private IWebDriver driver;
        private DriverManager driverManager;
        private HomePage homePage;
        private TestDataGenerator _testDataGenerator;

        [SetUp]
        public void Setup()
        {
            // Simple config - just read from appsettings.json
            driverManager = new DriverManager(Config.BrowserType, Config.BrowserHeadless);
            driver = driverManager.InitializeDriver();

            // Initialize pages and test data generator
            homePage = new HomePage(driver);
            _testDataGenerator = new TestDataGenerator();
        }

        [Test]
        public void Test_UserRegistration_WithValidData_Success()
        {
            // Arrange - Generate RANDOM user data (unique every time!)
            var newUser = _testDataGenerator.GenerateRandomUser();
            Console.WriteLine($"Registering new user: {newUser.Email}");

            // Act
            homePage.NavigateTo(Config.BaseUrl);
            var registerPage = homePage.ClickMyAccountRegister();
            registerPage.RegisterUser(newUser);

            // Assert
            bool isSuccessful = registerPage.IsRegistrationSuccessful();
            
            if (!isSuccessful)
            {
                // Debug: Print why it failed
                Console.WriteLine($"❌ Registration FAILED!");
                Console.WriteLine($"  Current URL: {registerPage.GetCurrentUrl()}");
                Console.WriteLine($"  Page Title: {registerPage.GetPageTitle()}");
                Console.WriteLine($"  Errors on page: {registerPage.GetErrorMessages()}");
            }
            
            Assert.That(isSuccessful, Is.True,
                "Registration should be successful with valid data");
            Assert.That(registerPage.GetCurrentUrl(), Does.Contain("account/success"),
                "Should redirect to success page after registration");

            Console.WriteLine($"✓ User registered successfully!");
            Console.WriteLine($"  Name: {newUser.FirstName} {newUser.LastName}");
            Console.WriteLine($"  Email: {newUser.Email}");
        }

        [TearDown]
        public void Teardown()
        {
            driver?.Dispose();
            driverManager?.QuitDriver();
        }
    }
}
