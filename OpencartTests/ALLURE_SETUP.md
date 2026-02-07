# Allure Reports Setup

## Quick Start

### 1. Install Allure CLI (One-time setup)

**macOS:**
```bash
brew install allure
```

**Windows:**
```bash
scoop install allure
```

**Linux:**
Download from: https://github.com/allure-framework/allure2/releases

### 2. Run Tests & View Report

```bash
# Run tests
dotnet test

# View report (auto-opens browser with clickable link)
allure serve allure-results
```

Or use the provided script:
```bash
./run-tests-with-report.sh
```

### 3. Console Output

After tests run, you'll see:
```
🧪 Running tests...
Passed!  - Failed: 0, Passed: 5

📊 Allure results generated in: allure-results/
🚀 Opening Allure report...
Generating report to temp directory...
Report successfully generated
Starting web server...
Server started at <http://localhost:xxxx>. Press <Ctrl+C> to exit
```

The link is clickable in most terminals!

## What You Get

- ✅ Beautiful interactive dashboard
- 📊 Test history & trends
- 📸 Automatic screenshots on failure
- 🏷️ Tags, severity levels, suites
- ⏱️ Duration graphs
- 📁 Step-by-step test breakdown

## Manual Commands

```bash
# Generate report without opening
allure generate allure-results --clean -o allure-report

# Open existing report
allure open allure-report

# Clean old results
rm -rf allure-results allure-report
```
