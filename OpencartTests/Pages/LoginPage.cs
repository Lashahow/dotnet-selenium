using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OpencartTests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        
        // Locators
        private By EmailInput => By.Id("input-email");
        private By PasswordInput => By.Id("input-password");
        private By LoginButton => By.CssSelector("input[type='submit']");
        private By ErrorMessage => By.CssSelector(".alert-danger");
        
        // Constructor
        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        
        // Actions
        public string GetCurrentUrl()
        {
            return _driver.Url;
        }
        
        public void EnterEmail(string email)
        {
            _driver.FindElement(EmailInput).SendKeys(email);
        }
        
        public void EnterPassword(string password)
        {
            _driver.FindElement(PasswordInput).SendKeys(password);
        }
        
        public void ClickLogin()
        {
            _driver.FindElement(LoginButton).Click();
        }
        
        public void Login(string email, string password)
        {
            EnterEmail(email);
            EnterPassword(password);
            ClickLogin();
        }
        
        public bool IsErrorDisplayed()
        {
            try
            {
                var element = _wait.Until(d => d.FindElement(ErrorMessage));
                return element.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        
        public string GetErrorMessage()
        {
            return _driver.FindElement(ErrorMessage).Text;
        }
    }
}