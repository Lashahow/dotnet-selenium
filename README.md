# Opencart Test Automation Framework

C#/.NET test automation framework covering UI, API, and Database testing layers using NUnit, Selenium, RestSharp, and SQLite.

## Project Structure

```
OpencartTests/
├── Configuration/        # Centralized config (appsettings.json)
├── Helpers/
│   ├── DriverManager.cs         # Browser factory (Chrome/Firefox/Edge)
│   ├── DbHelper.cs              # Database connection & query helper
│   ├── ExtentReportManager.cs   # HTML report generation (Singleton)
│   ├── TestData.cs              # Static test data
│   └── TestDataGenerator.cs     # Random data with Bogus
├── Pages/                # Page Object Model (POM)
│   ├── HomePage.cs
│   ├── LoginPage.cs
│   ├── RegisterPage.cs
│   └── SearchResultsPage.cs
├── Tests/
│   ├── OpencartTests.cs         # UI smoke & regression tests
│   ├── RegistrationTests.cs     # Registration with random data
│   ├── ApiTests/
│   │   └── ApiCrudTests.cs      # REST API CRUD tests
│   └── DatabaseTests/
│       ├── BaseDatabaseTest.cs  # In-memory SQLite setup/teardown
│       └── DatabaseCrudTests.cs # SQL CRUD & JOIN tests
└── appsettings.json
```

## Quick Start

```bash
# Install dependencies
dotnet restore

# Run all tests
dotnet test

# Run by category
dotnet test --filter "Category=Smoke"
dotnet test --filter "Category=API"
dotnet test --filter "Category=Database"

# Run with verbose output
dotnet test --logger "console;verbosity=detailed"
```

## Test Suites

### UI Tests (Selenium)
- Homepage title verification
- Navigation to login page
- Invalid login error handling
- Product search functionality
- User registration with random data (Bogus)

### API Tests (RestSharp)
- POST → GET → PUT → GET sequential CRUD flow
- Status code & response data validation
- State management across ordered tests

### Database Tests (SQLite)
- Schema verification via sqlite_master
- INSERT and verify with SELECT query
- UPDATE and verify changed value with ExecuteScalar
- DELETE with CASCADE — verify child rows removed
- INNER JOIN to verify parent-child relationship

## Configuration

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

Override at runtime:
```bash
export TestConfiguration__Browser__Headless=true
dotnet test
```

## Design Patterns

| Pattern | Where | Purpose |
|---------|-------|---------|
| **Page Object Model** | `Pages/*.cs` | Separates UI logic from tests |
| **Factory Pattern** | `DriverManager` | Multi-browser support |
| **Singleton Pattern** | `ExtentReportManager` | One shared HTML report |
| **Inheritance** | `BaseDatabaseTest` | Shared DB setup/teardown for all DB tests |
| **IDisposable** | `DbHelper` | Proper connection cleanup |

## Reporting

### Allure Reports
```bash
dotnet test --filter "Category=API"
allure serve allure-results
```

### ExtentReports
HTML reports auto-generated in `TestResults/Reports/` with timestamped filenames.

## Tech Stack

- .NET 10.0 | NUnit 4.3.2 | Selenium WebDriver 4.40.0
- RestSharp 113.1.0 | Microsoft.Data.Sqlite 10.0.2
- Bogus 35.6.5 | Allure.NUnit 2.14.1 | ExtentReports

## Documentation

- `PROJECT_PROGRESS.md` — Feature tracking & backlog
- `CONFIGURATION_SETUP.md` — Configuration details
