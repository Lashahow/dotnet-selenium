using Microsoft.Extensions.Configuration;

namespace OpencartTests.Configuration
{
    /// <summary>
    /// Helper class to load and manage test configuration from appsettings.json
    /// Implements lazy singleton pattern for performance
    /// </summary>
    public static class ConfigurationHelper
    {
        private static TestConfiguration? _configuration;
        private static readonly object _lock = new();

        /// <summary>
        /// Gets the test configuration. Loads from appsettings.json on first access.
        /// Supports environment-specific overrides (e.g., appsettings.Development.json)
        /// </summary>
        public static TestConfiguration GetConfiguration()
        {
            if (_configuration != null)
                return _configuration;

            lock (_lock)
            {
                if (_configuration != null)
                    return _configuration;

                // Determine environment (defaults to Development if not set)
                var environment = Environment.GetEnvironmentVariable("TEST_ENV") 
                    ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                    ?? "Development";

                Console.WriteLine($"Loading configuration for environment: {environment}");

                // Build configuration from JSON files and environment variables
                var configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
                    .AddEnvironmentVariables(); // Allow environment variable overrides

                var config = configBuilder.Build();

                // Bind configuration to strongly-typed object
                _configuration = new TestConfiguration();
                config.GetSection("TestConfiguration").Bind(_configuration);

                // Validate required settings
                ValidateConfiguration(_configuration);

                return _configuration;
            }
        }

        /// <summary>
        /// Validates that required configuration values are present
        /// </summary>
        private static void ValidateConfiguration(TestConfiguration config)
        {
            if (string.IsNullOrEmpty(config.BaseUrl))
                throw new InvalidOperationException("BaseUrl is required in test configuration");

            if (string.IsNullOrEmpty(config.Browser.Type))
                throw new InvalidOperationException("Browser.Type is required in test configuration");

            Console.WriteLine($"✓ Configuration loaded successfully");
            Console.WriteLine($"  - Base URL: {config.BaseUrl}");
            Console.WriteLine($"  - Browser: {config.Browser.Type} (Headless: {config.Browser.Headless})");
            Console.WriteLine($"  - Implicit Wait: {config.Timeouts.ImplicitWait}s");
        }

        /// <summary>
        /// Resets the cached configuration (useful for testing)
        /// </summary>
        public static void Reset()
        {
            lock (_lock)
            {
                _configuration = null;
            }
        }
    }
}
