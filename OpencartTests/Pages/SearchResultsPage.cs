using OpenQA.Selenium;

namespace OpencartTests.Pages
{
    public class SearchResultsPage
    {
        private readonly IWebDriver _driver;
        
        // Constructor
        public SearchResultsPage(IWebDriver driver)
        {
            _driver = driver;
        }
        
        // Actions
        public string GetPageSource()
        {
            return _driver.PageSource;
        }
        
        public bool IsProductDisplayed(string productName)
        {
            return _driver.PageSource.Contains(productName);
        }
    }
}