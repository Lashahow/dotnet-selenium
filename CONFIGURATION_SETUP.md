# Configuration Management

Centralized configuration using appsettings.json with Microsoft.Extensions.Configuration.

---

## 📁 Files Structure

```
OpencartTests/
├── Configuration/
│   ├── TestConfiguration.cs      # Strongly-typed models
│   └── ConfigurationHelper.cs    # Singleton loader
├── appsettings.json              # Default config
└── appsettings.Development.json  # Local overrides
```

---

## ⚙️ Configuration Hierarchy

**Precedence (highest to lowest):**
1. Environment variables (runtime)
2. appsettings.{Environment}.json
3. appsettings.json (default)

---

## 🔧 Usage Examples

### In Tests
```csharp
[SetUp]
public void Setup()
{
    var config = ConfigurationHelper.GetConfiguration(); // Singleton
    driver = new DriverManager(config.Browser.Type, config.Browser.Headless)
                 .InitializeDriver();
    homePage.NavigateTo(config.BaseUrl);
}
```

### Environment Switching
```bash
# Use Development (default)
dotnet test

# Use CI environment
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

## 📋 Configuration Structure

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
      "Valid": { "Email": "...", "Password": "..." },
      "Invalid": { "Email": "...", "Password": "..." }
    },
    "TestData": {
      "SearchProducts": { "MacBook": "MacBook", "iPhone": "iPhone" }
    }
  }
}
```

---

## 🔒 Security Best Practices

### Safe to Commit
✅ appsettings.json (with non-sensitive defaults)  
✅ appsettings.Development.json  
✅ appsettings.CI.json  

### Never Commit
❌ Real credentials  
❌ Production API keys  
❌ Database passwords  

### For Secrets
Use environment variables in CI/CD:
```bash
export TestConfiguration__TestUsers__Valid__Email=${{ secrets.TEST_EMAIL }}
export TestConfiguration__TestUsers__Valid__Password=${{ secrets.TEST_PASSWORD }}
```

---

## 🎓 Interview Talking Points

**"How do you manage test configuration?"**
*"I use appsettings.json with IConfiguration for layered, type-safe configuration. Strongly-typed models provide IntelliSense and compile-time safety. Environment-specific files override defaults, while CI/CD injects secrets via environment variables. This follows 12-factor app methodology and prevents credential leakage."*

**"Why not static classes or constants?"**
*"While simpler, appsettings.json provides: (1) Environment-specific overrides without code changes, (2) External config management, (3) Integration with .NET's configuration system, (4) Easier secret management. It's aligned with enterprise .NET practices."*

**"How do you handle parallel test execution?"**
*"ConfigurationHelper implements thread-safe singleton with double-check locking. Configuration loads once and is shared (read-only after initialization), preventing redundant I/O while ensuring consistency across parallel threads."*

---

## ✅ Implementation Details

### Packages Added
```xml
<PackageReference Include="Microsoft.Extensions.Configuration" Version="9.0.1" />
<PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.1" />
<PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="9.0.1" />
<PackageReference Include="Microsoft.Extensions.Configuration.EnvironmentVariables" Version="9.0.1" />
```

### Key Features
- Lazy singleton pattern (thread-safe)
- Environment auto-detection (TEST_ENV or DOTNET_ENVIRONMENT)
- Configuration validation on load
- Console output for debugging

---

**Status:** ✅ Complete and Production-Ready  
**Updated:** February 6, 2026
