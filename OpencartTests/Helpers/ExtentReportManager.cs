using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

namespace OpencartTests.Helpers
{
    // Singleton — one shared report instance across all test classes
    public static class ExtentReportManager
    {
        // Lazy<T> ensures thread-safe, one-time initialization
        private static readonly Lazy<ExtentReports> _lazyInstance = new(() => InitializeReport());

        // Full path to the generated HTML report file
        public static string ReportPath { get; private set; } = string.Empty;

        // Returns the single shared ExtentReports instance
        public static ExtentReports GetInstance() => _lazyInstance.Value;

        // Runs once on first access — sets up the HTML reporter
        private static ExtentReports InitializeReport()
        {
            // Build timestamped report path (each run gets its own file)
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string reportDirectory = Path.Combine(
                Directory.GetCurrentDirectory(), "TestResults", "Reports");

            Directory.CreateDirectory(reportDirectory);

            string reportFileName = $"ExtentReport_{timestamp}.html";
            ReportPath = Path.Combine(reportDirectory, reportFileName);

            // Configure the HTML reporter
            var sparkReporter = new ExtentSparkReporter(ReportPath);
            sparkReporter.Config.Theme = Theme.Dark;
            sparkReporter.Config.DocumentTitle = "Opencart Test Automation Report";
            sparkReporter.Config.ReportName = "Opencart Selenium Tests";
            sparkReporter.Config.Encoding = "utf-8";

            // Create report instance and attach reporter
            var extent = new ExtentReports();
            extent.AttachReporter(sparkReporter);

            // Add environment info (shown in report's Environment section)
            extent.AddSystemInfo("Application", "Opencart E-Commerce");
            extent.AddSystemInfo("Browser", Configuration.Config.BrowserType);
            extent.AddSystemInfo("Headless Mode", Configuration.Config.BrowserHeadless.ToString());
            extent.AddSystemInfo("Base URL", Configuration.Config.BaseUrl);
            extent.AddSystemInfo("OS", Environment.OSVersion.ToString());
            extent.AddSystemInfo(".NET Version", Environment.Version.ToString());
            extent.AddSystemInfo("Machine", Environment.MachineName);

            Console.WriteLine($"[ExtentReports] Report initialized: {ReportPath}");
            return extent;
        }

        // Writes all logged results to the HTML file — call once after all tests
        public static void FlushReport()
        {
            if (_lazyInstance.IsValueCreated)
            {
                _lazyInstance.Value.Flush();
                Console.WriteLine($"[ExtentReports] Report saved to: {ReportPath}");
            }
        }
    }
}
