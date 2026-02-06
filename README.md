# Opencart Test Automation Framework

C#/.NET test automation framework using NUnit, Selenium WebDriver, and Page Object Model for testing the Opencart e-commerce application, plus REST API testing.

## 🏗️ Project Structure

```
OpencartTests/
├── Configuration/        # Centralized config (appsettings.json)
├── Helpers/             # DriverManager, TestDataGenerator
├── Pages/               # Page Object Model (POM)
├── Tests/
│   ├── OpencartTests.cs        # UI tests
│   ├── RegistrationTests.cs    # Registration tests
│   └── ApiTests/
│       └── ApiCrudTests.cs     # REST API tests
└── appsettings.json     # Configuration
```

## 🚀 Quick Start

```bash
# Install dependencies
dotnet restore

# Run all tests
dotnet test

# Run by category
dotnet test --filter "Category=Smoke"
dotnet test --filter "Category=API"

# Run with verbose output
dotnet test --logger "console;verbosity=detailed"
```

## ⚙️ Configuration

**Layered approach:** appsettings.json → Environment-specific JSON → Environment variables

```json
{
  "TestConfiguration": {
    "BaseUrl": "https://naveenautomationlabs.com/opencart",
    "Browser": { "Type": "chrome", "Headless": false },
    "Timeouts": { "ImplicitWait": 10, "ExplicitWait": 20 }
  }
}
```

**Override example:**
```bash
export TestConfiguration__Browser__Headless=true
dotnet test
```

## 🎨 Design Patterns & Features

| Pattern | Implementation | Purpose |
|---------|---------------|---------|
| **Page Object Model** | `Pages/*.cs` | Separates UI logic from tests |
| **Factory Pattern** | `DriverManager` | Multi-browser support (Chrome/Firefox/Edge) |
| **Singleton Pattern** | `ConfigurationHelper` | Thread-safe config management |
| **Test Data Generation** | `Bogus library` | Random data per test run |

**Key Features:**
- Multi-browser support with auto-detection of CI environment
- Strongly-typed configuration with environment overrides
- Explicit waits for dynamic elements
- Sequential API testing (POST → GET → PUT → GET)

## 🧪 Test Suites

### UI Tests (Selenium)
- **Smoke:** Homepage title, navigation
- **Regression:** Login, search, registration
- Uses Bogus for random test data

### API Tests (RestSharp)
- **CRUD operations** on REST API
- Sequential execution with `[Order]`
- Status code & data validation
- Demonstrates code reusability (DRY principle)

```bash
dotnet test --filter "Category=Smoke"
dotnet test --filter "Category=API"
```

## 💬 Interview Talking Points

**Configuration Management:**
*"I use appsettings.json with IConfiguration for type-safe, environment-aware config. This follows .NET best practices and 12-factor app methodology. Same tests run against Dev/Staging/Prod without code changes. Secrets injected via environment variables in CI/CD."*

**Page Object Model:**
*"POM encapsulates page logic, making tests maintainable. If a locator changes, I update one place. Tests describe business workflow, not technical implementation. Follows Single Responsibility Principle."*

**Factory Pattern (DriverManager):**
*"Centralizes WebDriver configuration—multi-browser support, environment detection, browser options in one place. Makes it trivial to switch browsers or add CI-specific flags."*

**API Testing Approach:**
*"Sequential CRUD tests with state management. Reuse GET test for verification after PUT (DRY principle). This verifies actual persistence, not just PUT response. Mirrors real-world usage patterns."*

## 🛠️ Tech Stack

- .NET 10.0 | NUnit 4.3.2 | Selenium WebDriver 4.40.0
- RestSharp 113.1.0 | Bogus 35.6.5
- Microsoft.Extensions.Configuration 9.0.1

## 📄 Documentation

- `PROJECT_PROGRESS.md` - Feature tracking
- `CONFIGURATION_SETUP.md` - Config details
