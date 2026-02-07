using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using OpencartTests.Helpers;
using OpencartTests.Configuration;

namespace OpencartTests.Tests
{
    [AllureNUnit]
    public abstract class BaseTest
    {
        protected IWebDriver driver = null!;
        protected DriverManager driverManager = null!;

        [SetUp]
        public void SetUpBase()
        {
            driverManager = new DriverManager(Config.BrowserType, Config.BrowserHeadless);
            driver = driverManager.InitializeDriver();
        }

        [TearDown]
        public void TearDownBase()
        {
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;
            
            if (outcome == TestStatus.Failed && driver != null)
            {
                try
                {
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    AllureApi.AddAttachment("Screenshot on Failure", "image/png", screenshot.AsByteArray);
                }
                catch { }
            }

            driver?.Dispose();
            driverManager?.QuitDriver();
        }
    }
}
