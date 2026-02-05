using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;

namespace OpencartTests.Helpers
{
    public class DriverManager
    {
        private IWebDriver? _driver;
        private readonly string _browserType;
        private readonly bool _headless;
        
        // Constructor with dependency injection pattern
        public DriverManager(string browserType = "chrome", bool? headless = null)
        {
            _browserType = browserType;
            // If headless is not specified, check CI environment
            _headless = headless ?? IsRunningInCI();
        }
        
        public IWebDriver InitializeDriver()
        {
            // Factory pattern - create browser based on type
            _driver = _browserType.ToLower() switch
            {
                "chrome" => new ChromeDriver(GetChromeOptions()),
                "firefox" => new FirefoxDriver(GetFirefoxOptions()),
                "edge" => new EdgeDriver(GetEdgeOptions()),
                _ => throw new ArgumentException($"Browser '{_browserType}' is not supported. Use: chrome, firefox, or edge")
            };
            
            ConfigureDriverTimeouts(_driver);
            ConfigureDriverWindow(_driver);
            
            return _driver;
        }
        
        private ChromeOptions GetChromeOptions()
        {
            var options = new ChromeOptions();
            
            // Headless mode configuration
            if (_headless)
            {
                options.AddArgument("--headless=new"); // Use new headless mode (Chrome 109+)
                options.AddArgument("--window-size=1920,1080"); // Set viewport in headless
                options.AddArgument("--no-sandbox"); // Required for Docker/CI
                options.AddArgument("--disable-dev-shm-usage"); // Prevent crashes in limited memory
            }
            else
            {
                options.AddArgument("--start-maximized"); // Maximize browser window
            }
            
            // Security & stability
            options.AddArgument("--disable-notifications"); // Block browser notifications
            options.AddArgument("--disable-popup-blocking"); // Allow popups for testing
            options.AddArgument("--disable-blink-features=AutomationControlled"); // Hide "Chrome is being controlled" banner
            
            // Performance optimizations
            options.AddArgument("--disable-extensions"); // Disable Chrome extensions
            options.AddArgument("--disable-gpu"); // Disable GPU acceleration (stability in CI)
            options.AddArgument("--disable-web-security"); // Allow cross-origin requests (testing only!)
            
            // User preferences
            options.AddUserProfilePreference("credentials_enable_service", false); // Don't save passwords
            options.AddUserProfilePreference("profile.password_manager_enabled", false); // Disable password manager
            
            // Download configuration
            var downloadPath = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
            Directory.CreateDirectory(downloadPath); // Create folder if not exists
            options.AddUserProfilePreference("download.default_directory", downloadPath);
            options.AddUserProfilePreference("download.prompt_for_download", false); // Auto-download
            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true); // Download PDFs
            
            // Hide automation flags (bypass bot detection)
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);
            
            // Logging (helpful for debugging)
            options.SetLoggingPreference(LogType.Browser, LogLevel.Info);
            
            return options;
        }
        
        private FirefoxOptions GetFirefoxOptions()
        {
            var options = new FirefoxOptions();
            
            if (_headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("--width=1920");
                options.AddArgument("--height=1080");
            }
            
            // Set preferences
            options.SetPreference("dom.webnotifications.enabled", false);
            options.SetPreference("geo.enabled", false);
            
            return options;
        }
        
        private EdgeOptions GetEdgeOptions()
        {
            var options = new EdgeOptions();
            
            if (_headless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--window-size=1920,1080");
            }
            else
            {
                options.AddArgument("--start-maximized");
            }
            
            options.AddArgument("--disable-notifications");
            
            return options;
        }
        
        private void ConfigureDriverTimeouts(IWebDriver driver)
        {
            // Implicit wait - time to wait when finding elements
            // NOTE: Modern practice favors explicit waits (WebDriverWait)
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            
            // Page load timeout - max time to wait for page to load
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            
            // Async JavaScript timeout - for ExecuteAsyncScript
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);
        }
        
        private void ConfigureDriverWindow(IWebDriver driver)
        {
            // Only maximize if not in headless mode
            if (!_headless)
            {
                driver.Manage().Window.Maximize();
            }
        }
        
        private bool IsRunningInCI()
        {
            // Check common CI environment variables
            return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI")) ||
                   !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS")) ||
                   !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("JENKINS_HOME"));
        }
        
        public void QuitDriver()
        {
            // Safely dispose of driver
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }
    }
}