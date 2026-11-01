#!/bin/bash
set -e

echo "Running Flutter mobile unit tests..."
cd my_app && flutter pub get && flutter test --coverage
cd ..

echo "Running .NET C# engine tests..."
dotnet test Assets/Tests/GameTests.csproj --logger "console;verbosity=normal"

echo "All tests passed successfully!"
