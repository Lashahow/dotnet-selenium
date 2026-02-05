using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace OpencartTests.Helpers
{
    public class DriverManager
    {
        private IWebDriver? _driver;
        
        public IWebDriver InitializeDriver()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            return _driver;
        }
            
        public void QuitDriver()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}