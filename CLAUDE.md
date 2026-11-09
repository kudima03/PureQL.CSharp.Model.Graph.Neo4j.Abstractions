# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All `dotnet` commands must be run from the `./src` directory. The SDK is pinned by `global.json` (.NET 11, for the model's C# 15 unions); where the system SDK is older, use `~/.dotnet/dotnet` or put it first on `PATH` with `DOTNET_ROOT=$HOME/.dotnet`.

```bash
dotnet restore
dotnet build --no-restore -warnaserror
dotnet format --verify-no-changes          # check code style (CI enforces this)
dotnet format                              # auto-fix code style
dotnet test --no-build --verbosity normal  # run tests
dotnet csharpier check .                   # check formatting (CI enforces this)
dotnet pack --configuration Release -p:Version=<version> --output .
```

CI additionally passes `-p:AssemblyVersion` (pinned to the major) and `-p:FileVersion`; see `.github/workflows/publish-nuget.yml`.

## Architecture

This is the **Neo4j abstractions package** of the `PureQL.CSharp.Model.Graph` family: the contract the Neo4j operation packages (Creation, Insertion, Querying, Deletion, Updating, Existence, Destruction, Migration) share, so that they depend on it rather than on each other. It depends on `PureQL.CSharp.Model` `0.1.0-preview.14.0.0` and `Neo4j.Driver` `6.5.1` (it follows the driver's latest stable major), and holds no I/O. Namespace `PureQL.CSharp.Model.Graph.Neo4j.Abstractions`, one public type per file:

- `INeo4jStoredQueries` — `IDriver Driver` and `string Database`: the Neo4j database holding the query graphs.
- `Neo4jStoredQueries` — `sealed record (IDriver Driver, string Database) : INeo4jStoredQueries`; the `(IDriver driver)` overload uses the constant `DefaultDatabase` (`"neo4j"`, the only user database on Community edition). It does not own the driver.
- `StoredQuery` — `sealed record (Guid Id, PureQLQuery Query)`: a query as stored, its id and the query.

The operation packages run their statements on an `IAsyncQueryRunner` (an `IAsyncSession` or an `IAsyncTransaction`), so operations compose inside one transaction, and offer convenience records over `INeo4jStoredQueries` that open the session on `Driver` against `Database`. The public API is the contract those packages are written against: do not rename or reshape it, and add to it only what they all need. Every public member has XML documentation (`GenerateDocumentationFile`; a missing comment fails the build under `-warnaserror`).

**Test project:** `PureQL.CSharp.Model.Graph.Neo4j.Abstractions.Tests` targets `net11.0` and uses xunit with coverlet. It checks the records' values, equality, `with` expressions and the default database over `FakeDriver`, an `IDriver` stub whose members throw; no Neo4j server is needed.

**Target framework:** net11.0 only, as the model. `IsAotCompatible` is `true`.

**Package validation:** enabled against `PackageValidationBaselineVersion` `0.1.0-preview.0.1.0`, the first release; the `nuget-publish` skill moves it after each release, and intentional breaks go to `CompatibilitySuppressions.xml` next to the csproj.

**Mutation testing:** the Stryker step of CI is commented out.

**Publishing:** triggered by pushing a semver tag (pattern `*.*.*`). The tag becomes the `PackageVersion`. The workflow publishes to both GitHub Packages and NuGet.org.

## Code Style

Enforced via `.editorconfig` and `dotnet format --verify-no-changes` in CI:

- No `var` — always use explicit types
- No expression-bodied methods or constructors — use block bodies
- Expression-bodied properties and accessors are required
- File-scoped namespace declarations (`namespace Foo.Bar;`)
- Private fields prefixed with `_` in camelCase (e.g., `_options`)
- Braces always required, even for single-statement blocks
- Max line length: 90 characters
- Allman brace style — opening brace on its own line for all blocks
- `System.*` using directives sorted first, no blank line between using groups
- Parentheses required in arithmetic and relational binary operators for clarity

## Commit Messages

Do not mention Claude or AI assistance in commit messages.
