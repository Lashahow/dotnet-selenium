using OpenQA.Selenium;

namespace OpencartTests.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        
        // Locators
        private By MyAccountDropdown => By.LinkText("My Account");
        private By LoginLink => By.LinkText("Login");
        private By RegisterLink => By.LinkText("Register");
        private By SearchBox => By.Name("search");
        private By SearchButton => By.CssSelector("button.btn-default");
        
        // Constructor
        public HomePage(IWebDriver driver)
        {
            _driver = driver;
        }
        
        // Actions
        public void NavigateTo(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }
        
        public string GetTitle()
        {
            return _driver.Title;
        }
        
        public LoginPage ClickMyAccountLogin()
        {
            _driver.FindElement(MyAccountDropdown).Click();
            _driver.FindElement(LoginLink).Click();
            return new LoginPage(_driver);
        }

        public RegisterPage ClickMyAccountRegister()
        {
            _driver.FindElement(MyAccountDropdown).Click();
            _driver.FindElement(RegisterLink).Click();
            return new RegisterPage(_driver);
        }
        
        public SearchResultsPage SearchProduct(string productName)
        {
            _driver.FindElement(SearchBox).SendKeys(productName);
            _driver.FindElement(SearchButton).Click();
            return new SearchResultsPage(_driver);
        }
    }
}