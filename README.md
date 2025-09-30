# FaDiesis - Learning F#, hands on

## Creating the solution

```bash
dotnet new sln --name FaDiesis
mkdir src
dotnet new xunit --language F# -o src/FSTest -n FSTest
dotnet new xunit --language C# -o src/CSTest -n CSTest
dotnet sln add src/CSTest/CSTest.csproj
dotnet sln add src/FSTest/FSTest.fsproj
```

* [Day 1](docs/day-1.md)
