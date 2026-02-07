#!/bin/bash

echo "🧪 Running tests..."
dotnet test --no-build

echo ""
echo "📊 Allure results generated in: allure-results/"
echo ""

# Check if allure is installed
if command -v allure &> /dev/null; then
    echo "🚀 Opening Allure report..."
    allure serve allure-results
else
    echo "⚠️  Allure CLI not found. Install it first:"
    echo ""
    echo "   macOS:   brew install allure"
    echo "   Windows: scoop install allure"
    echo "   Linux:   Download from https://github.com/allure-framework/allure2/releases"
    echo ""
    echo "After installing, run: allure serve allure-results"
fi
