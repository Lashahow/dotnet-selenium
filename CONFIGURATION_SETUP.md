# Configuration Management

Centralized configuration using appsettings.json with Microsoft.Extensions.Configuration.

---

## Files Structure

```
OpencartTests/
├── Configuration/
│   └── Config.cs                 # Static config loader
├── appsettings.json              # Default config
└── appsettings.Development.json  # Local overrides
```

---

## Configuration Hierarchy

**Precedence (highest to lowest):**
1. Environment variables (runtime)
2. appsettings.{Environment}.json
3. appsettings.json (default)

---

## Usage Examples

### In Tests
```csharp
[SetUp]
public void Setup()
{
    driverManager = new DriverManager(Config.BrowserType, Config.BrowserHeadless);
    driver = driverManager.InitializeDriver();
    homePage = new HomePage(driver);
}
```

### Environment Switching
```bash
# Default
dotnet test

# CI environment
export TEST_ENV=CI
dotnet test
```

### Environment Variable Override
```bash
# Override base URL
export TestConfiguration__BaseUrl=https://staging.opencart.com

# Override browser settings
export TestConfiguration__Browser__Type=firefox
export TestConfiguration__Browser__Headless=true

dotnet test
```

**Note:** Use double underscore `__` for nested properties.

---

## Configuration Structure

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
    }
  }
}
```

---

## Security Best Practices

### Safe to Commit
- appsettings.json (with non-sensitive defaults)
- appsettings.Development.json
- appsettings.CI.json

### Never Commit
- Real credentials
- Production API keys
- Database passwords

### For Secrets
Use environment variables in CI/CD:
```bash
export TestConfiguration__TestUsers__Valid__Email=${{ secrets.TEST_EMAIL }}
export TestConfiguration__TestUsers__Valid__Password=${{ secrets.TEST_PASSWORD }}
```

---

## Packages

```xml
<PackageReference Include="Microsoft.Extensions.Configuration" Version="9.0.1" />
<PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.1" />
<PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="9.0.1" />
<PackageReference Include="Microsoft.Extensions.Configuration.EnvironmentVariables" Version="9.0.1" />
```

---

**Updated:** February 7, 2026
