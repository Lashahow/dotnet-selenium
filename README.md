# Opencart Test Automation Framework

A robust C#/.NET test automation framework using NUnit, Selenium WebDriver, and Page Object Model pattern for testing the Opencart e-commerce application.

## 🏗️ Architecture

```
OpencartTests/
├── Configuration/           # Centralized configuration management
│   ├── TestConfiguration.cs       # Strongly-typed configuration models
│   └── ConfigurationHelper.cs     # Configuration loader with singleton pattern
├── Helpers/                 # Utility classes
│   └── DriverManager.cs           # WebDriver factory with multi-browser support
├── Pages/                   # Page Object Model classes
│   ├── HomePage.cs
│   ├── LoginPage.cs
│   └── SearchResultsPage.cs
├── Tests/                   # Test classes
│   └── OpencartTests.cs           # Main test suite
├── appsettings.json         # Default configuration
├── appsettings.Development.json   # Local development overrides
└── OpencartTests.csproj     # Project file with dependencies
```

## 🚀 Getting Started

### Prerequisites
- .NET 10.0 SDK or later
- Chrome browser installed
- Visual Studio 2022, VS Code, or Rider (optional)

### Installation

1. Clone the repository
2. Restore NuGet packages:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

## 🧪 Running Tests

### Run all tests:
```bash
dotnet test
```

### Run with detailed output:
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Run specific test by name:
```bash
dotnet test --filter "Name~VerifyTitle"
```

### Run tests by category:
```bash
# Run smoke tests only
dotnet test --filter "TestCategory=Smoke"

# Run regression tests only
dotnet test --filter "TestCategory=Regression"
```

### Run a single test method:
```bash
dotnet test --filter "FullyQualifiedName=OpencartTests.Tests.OpencartTests.Test_HomePage_VerifyTitle"
```

## ⚙️ Configuration

### Configuration Files

The framework uses a layered configuration approach:

1. **appsettings.json** - Default configuration (committed to git)
2. **appsettings.{Environment}.json** - Environment-specific overrides
3. **Environment variables** - Runtime overrides (highest priority)

### Configuration Structure

```json
{
  "TestConfiguration": {
    "BaseUrl": "https://naveenautomationlabs.com/opencart",
    "Timeouts": {
      "ImplicitWait": 10,
      "PageLoadTimeout": 30,
      "ExplicitWait": 20
    },
    "Browser": {
      "Type": "chrome",
      "Headless": false
    },
    "TestUsers": {
      "Valid": {
        "Email": "testuser@opencart.com",
        "Password": "ValidPassword123"
      },
      "Invalid": {
        "Email": "invalid@test.com",
        "Password": "wrongpassword"
      }
    }
  }
}
```

### Environment-Specific Configuration

To use a different environment:

```bash
# Use Development settings (default)
dotnet test

# Use CI settings
export TEST_ENV=CI
dotnet test

# Use Production settings
export TEST_ENV=Production
dotnet test
```

### Override with Environment Variables

You can override any configuration value using environment variables:

```bash
export TestConfiguration__BaseUrl=https://staging.opencart.com
export TestConfiguration__Browser__Headless=true
dotnet test
```

## 🎨 Design Patterns

### 1. Page Object Model (POM)
Encapsulates web page elements and actions, separating test logic from page interactions.

```csharp
// Example: LoginPage
public class LoginPage
{
    private readonly IWebDriver _driver;
    private By EmailInput => By.Id("input-email");
    
    public void Login(string email, string password)
    {
        _driver.FindElement(EmailInput).SendKeys(email);
        // ... more actions
    }
}
```

### 2. Factory Pattern
DriverManager creates appropriate browser instances based on configuration.

```csharp
var driver = browserType switch
{
    "chrome" => new ChromeDriver(GetChromeOptions()),
    "firefox" => new FirefoxDriver(GetFirefoxOptions()),
    _ => throw new ArgumentException("Unsupported browser")
};
```

### 3. Singleton Pattern
ConfigurationHelper ensures single instance of configuration across all tests.

```csharp
public static TestConfiguration GetConfiguration()
{
    if (_configuration != null)
        return _configuration;
    
    lock (_lock)
    {
        // Load configuration once
    }
}
```

## 🔧 Browser Configuration

### Supported Browsers
- Chrome (default)
- Firefox
- Microsoft Edge

### Browser Features
- **Headless mode** - Auto-enabled in CI/CD environments
- **Download management** - Files auto-download to `Downloads/` folder
- **Popup blocking** - Notifications and password save dialogs disabled
- **Bot detection bypass** - Automation flags hidden
- **CI/CD optimized** - Docker-compatible flags (--no-sandbox, --disable-dev-shm-usage)

### Switching Browsers

In `appsettings.json`:
```json
{
  "Browser": {
    "Type": "firefox",
    "Headless": true
  }
}
```

Or via environment variable:
```bash
export TestConfiguration__Browser__Type=edge
```

## 📊 Test Categories

### Smoke Tests
Quick validation of critical functionality:
- Homepage title verification
- Login page navigation

### Regression Tests
Comprehensive feature testing:
- Login with invalid credentials
- Product search functionality

Run by category:
```bash
dotnet test --filter "TestCategory=Smoke"
```

## 🔍 Key Features

### 1. **Centralized Configuration**
- No hardcoded URLs or credentials in tests
- Easy environment switching
- Type-safe configuration access

### 2. **Multi-Browser Support**
- Chrome, Firefox, Edge support
- Easy to add more browsers
- Consistent behavior across browsers

### 3. **CI/CD Ready**
- Auto-detection of CI environments
- Headless mode for pipeline execution
- Docker-compatible browser flags

### 4. **Maintainable Architecture**
- Page Object Model for reusability
- Separation of concerns
- Clear folder structure

### 5. **Explicit Waits**
- WebDriverWait in LoginPage for dynamic elements
- Proper timeout handling
- Reduced flakiness

## 📝 Interview Talking Points

### Why Configuration as Code?
*"I implemented appsettings.json with IConfiguration for type-safe, environment-aware configuration. This follows .NET best practices and the 12-factor app methodology. Unlike hardcoded values, this approach allows the same test suite to run against Dev, Staging, and Production without code changes, with secrets injected via environment variables in CI/CD."*

### Why Page Object Model?
*"POM encapsulates page-specific logic, making tests more maintainable and readable. If a locator changes, I update it in one place. Tests describe the business workflow, not the technical implementation. This follows the Single Responsibility Principle and makes tests resilient to UI changes."*

### Why DriverManager?
*"DriverManager implements the Factory pattern to centralize WebDriver configuration. It handles multi-browser support, environment detection, and browser options in one place. This prevents test pollution with infrastructure code and makes it trivial to switch browsers or add CI-specific flags."*

### Implicit vs Explicit Waits?
*"I use both strategically: implicit waits (10s) provide a baseline for all element lookups, while explicit waits with WebDriverWait handle specific conditions like error messages appearing. Modern practice favors explicit waits for predictability, but implicit waits reduce boilerplate for standard scenarios."*

## 🛠️ Tech Stack

- **Framework:** .NET 10.0
- **Testing Framework:** NUnit 4.3.2
- **Browser Automation:** Selenium WebDriver 4.40.0
- **Configuration:** Microsoft.Extensions.Configuration 9.0.1
- **Test Runner:** NUnit3TestAdapter 5.0.0

## 📈 Future Enhancements

See `PROJECT_PROGRESS.md` for planned features including:
- Screenshot capture on failure
- Parallel test execution
- HTML reporting
- Data-driven tests
- BaseTest class
- Logging framework integration

## 📄 License

This is a demo project for interview purposes.
