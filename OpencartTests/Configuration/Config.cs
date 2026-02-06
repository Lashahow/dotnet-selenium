using Microsoft.Extensions.Configuration;

namespace OpencartTests.Configuration
{
    /// <summary>
    /// Simple configuration helper - reads appsettings.json
    /// Just 3 properties: BaseUrl, BrowserType, BrowserHeadless
    /// </summary>
    public static class Config
    {
        private static IConfiguration? _configuration;

        private static IConfiguration GetConfiguration()
        {
            if (_configuration != null)
                return _configuration;

            // Load appsettings.json
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables() // Allow override from env vars
                .Build();

            Console.WriteLine($"✓ Configuration loaded");
            Console.WriteLine($"  - Base URL: {BaseUrl}");
            Console.WriteLine($"  - Browser: {BrowserType} (Headless: {BrowserHeadless})");

            return _configuration;
        }

        // Simple properties - read directly from JSON
        public static string BaseUrl => GetConfiguration()["TestConfiguration:BaseUrl"] 
            ?? throw new Exception("BaseUrl not found in appsettings.json");

        public static string BrowserType => GetConfiguration()["TestConfiguration:Browser:Type"] ?? "chrome";

        public static bool BrowserHeadless
        {
            get
            {
                var value = GetConfiguration()["TestConfiguration:Browser:Headless"];
                return value != null && bool.Parse(value);
            }
        }
    }
}
