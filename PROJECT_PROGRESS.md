# Project Progress

NUnit + Selenium + RestSharp + SQLite test automation framework for technical interview preparation.

---

## Completed Features

### Infrastructure
- DriverManager (Factory Pattern) — multi-browser support (Chrome/Firefox/Edge)
- CI/CD auto-detection with headless mode
- Configuration via appsettings.json with environment overrides
- TestDataGenerator with Bogus library
- DbHelper — SQLite connection management with IDisposable
- ExtentReportManager (Singleton) — HTML test reports

### Page Objects
- HomePage, LoginPage, SearchResultsPage, RegisterPage
- Explicit waits (WebDriverWait) for dynamic elements

### Test Suites

**UI Tests (5 tests)**
- Homepage title verification (Smoke)
- Navigation to login page (Smoke)
- Invalid login error handling (Regression)
- Product search functionality (Regression)
- User registration with random data (Regression)

**API Tests (4 tests)**
- POST — Create object with Bogus data
- GET — Retrieve and validate created object
- PUT — Update with additional fields
- GET — Verify updated data persists

**Database Tests (5 tests)**
- Verify tables exist via sqlite_master
- INSERT policy and verify with SELECT
- UPDATE premium and verify with ExecuteScalar
- DELETE policy and verify CASCADE removes claims
- INSERT policy + claim and verify with JOIN query

### Reporting
- Allure Report integration
- ExtentReports HTML generation (dark theme, timestamped files)
- Test suites grouping (UI, API, Database)

---

## Design Patterns Demonstrated

| Pattern | Implementation |
|---------|----------------|
| **Page Object Model** | Separates UI logic from tests |
| **Factory Pattern** | DriverManager creates browsers |
| **Singleton Pattern** | ExtentReportManager |
| **Inheritance** | BaseDatabaseTest → DatabaseCrudTests |
| **IDisposable** | DbHelper connection cleanup |
| **DRY Principle** | Reuse GET test for API verification |
| **Sequential Testing** | [Order] attribute in API and DB tests |

---

## Backlog

**High Priority**
- [ ] Screenshot capture on test failure
- [ ] BaseTest class to reduce UI test duplication
- [ ] Data-driven tests with [TestCase]
- [x] ~~HTML report generation~~ (Done — Allure + ExtentReports)
- [ ] Parallel test execution

**Medium Priority**
- [ ] Logging framework (Serilog/NLog)
- [ ] Fluent assertions
- [ ] More page objects (Cart, Checkout, Dashboard)

**Low Priority**
- [ ] Docker support
- [ ] GitHub Actions CI/CD pipeline
- [ ] Code coverage reporting

---

## Test Results

**Last Run:** All 14 tests passed

| Suite | Tests | Time |
|-------|-------|------|
| UI Tests | 5/5 | ~24s |
| API Tests | 4/4 | ~2s |
| Database Tests | 5/5 | <1s |

---

**Status:** Production-ready for interview demonstration
**Updated:** February 7, 2026
