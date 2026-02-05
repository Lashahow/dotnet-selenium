namespace OpencartTests.Configuration
{
    /// <summary>
    /// Main configuration class for test settings
    /// </summary>
    public class TestConfiguration
    {
        public string BaseUrl { get; set; } = string.Empty;
        public TimeoutSettings Timeouts { get; set; } = new();
        public BrowserSettings Browser { get; set; } = new();
        public TestUsersSettings TestUsers { get; set; } = new();
        public TestDataSettings TestData { get; set; } = new();
    }

    /// <summary>
    /// Timeout configuration for WebDriver waits
    /// </summary>
    public class TimeoutSettings
    {
        public int ImplicitWait { get; set; } = 10;
        public int PageLoadTimeout { get; set; } = 30;
        public int ExplicitWait { get; set; } = 20;
    }

    /// <summary>
    /// Browser launch configuration
    /// </summary>
    public class BrowserSettings
    {
        public string Type { get; set; } = "chrome";
        public bool Headless { get; set; } = false;
    }

    /// <summary>
    /// Test user credentials
    /// </summary>
    public class TestUsersSettings
    {
        public UserCredentials Valid { get; set; } = new();
        public UserCredentials Invalid { get; set; } = new();
    }

    /// <summary>
    /// User credentials model
    /// </summary>
    public class UserCredentials
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Test data for various test scenarios
    /// </summary>
    public class TestDataSettings
    {
        public SearchProductsSettings SearchProducts { get; set; } = new();
    }

    /// <summary>
    /// Product names for search tests
    /// </summary>
    public class SearchProductsSettings
    {
        public string MacBook { get; set; } = string.Empty;
        public string iPhone { get; set; } = string.Empty;
        public string Canon { get; set; } = string.Empty;
    }
}
