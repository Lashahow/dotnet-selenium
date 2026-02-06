using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpencartTests.Helpers;

namespace OpencartTests.Pages
{
    /// <summary>
    /// Page Object for the Registration page
    /// </summary>
    public class RegisterPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Locators
        private By FirstNameInput => By.Id("input-firstname");
        private By LastNameInput => By.Id("input-lastname");
        private By EmailInput => By.Id("input-email");
        private By TelephoneInput => By.Id("input-telephone");
        private By PasswordInput => By.Id("input-password");
        private By ConfirmPasswordInput => By.Id("input-confirm");
        private By PrivacyPolicyCheckbox => By.Name("agree");
        private By ContinueButton => By.CssSelector("input[value='Continue']");
        private By SuccessHeading => By.CssSelector("#content h1");
        private By ErrorMessages => By.CssSelector(".text-danger");

        public RegisterPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Fill and submit registration form
        /// </summary>
        public void RegisterUser(UserRegistrationData user)
        {
            _driver.FindElement(FirstNameInput).SendKeys(user.FirstName);
            _driver.FindElement(LastNameInput).SendKeys(user.LastName);
            _driver.FindElement(EmailInput).SendKeys(user.Email);
            _driver.FindElement(TelephoneInput).SendKeys(user.Telephone);
            _driver.FindElement(PasswordInput).SendKeys(user.Password);
            _driver.FindElement(ConfirmPasswordInput).SendKeys(user.Password);
            _driver.FindElement(PrivacyPolicyCheckbox).Click();
            _driver.FindElement(ContinueButton).Click();
            
            // Wait a moment for page to process
            System.Threading.Thread.Sleep(2000);
        }

        /// <summary>
        /// Check if registration was successful
        /// Waits for BOTH element AND text to be present (more stable!)
        /// </summary>
        public bool IsRegistrationSuccessful()
        {
            try
            {
                // Wait for element to exist AND text to contain expected value
                _wait.Until(d =>
                {
                    try
                    {
                        var element = d.FindElement(SuccessHeading);
                        return element.Displayed && element.Text.Contains("Your Account Has Been Created");
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public string GetCurrentUrl()
        {
            return _driver.Url;
        }

        /// <summary>
        /// Get any error messages on the page (for debugging flaky tests)
        /// </summary>
        public string GetErrorMessages()
        {
            try
            {
                var errors = _driver.FindElements(ErrorMessages);
                if (errors.Count > 0)
                {
                    return string.Join("; ", errors.Select(e => e.Text));
                }
                return "No errors found";
            }
            catch
            {
                return "Could not read errors";
            }
        }

        /// <summary>
        /// Get page title for debugging
        /// </summary>
        public string GetPageTitle()
        {
            try
            {
                var heading = _driver.FindElement(SuccessHeading);
                return heading.Text;
            }
            catch
            {
                return "Heading not found";
            }
        }
    }
}
