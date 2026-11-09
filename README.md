# PureQL.CSharp.Model.Graph.Neo4j.Abstractions

The Neo4j stored-queries contract shared by the **PureQL** graph storage packages.

[![.NET build & test](https://github.com/kudima03/PureQL.CSharp.Model.Graph.Neo4j.Abstractions/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/kudima03/PureQL.CSharp.Model.Graph.Neo4j.Abstractions/actions/workflows/build-and-test.yml)
[![Build and Deploy](https://github.com/kudima03/PureQL.CSharp.Model.Graph.Neo4j.Abstractions/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/PureQL.CSharp.Model.Graph.Neo4j.Abstractions/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/PureQL.CSharp.Model.Graph.Neo4j.Abstractions)](https://www.nuget.org/packages/PureQL.CSharp.Model.Graph.Neo4j.Abstractions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Overview

`PureQL.CSharp.Model.Graph.Neo4j.Abstractions` is the small package that the Neo4j operation packages of the `PureQL.CSharp.Model.Graph` family share: one description of the Neo4j database holding PureQL query graphs and one record of a stored query. The operation packages depend on it instead of on each other, so a consumer can combine any of them over the same database.

## Public API

Namespace `PureQL.CSharp.Model.Graph.Neo4j.Abstractions`:

| Name | Description |
|------|-------------|
| `INeo4jStoredQueries` | A Neo4j database holding PureQL query graphs: the `IDriver Driver` connected to the server and the `string Database` name |
| `Neo4jStoredQueries` | `sealed record Neo4jStoredQueries(IDriver Driver, string Database)`, the implementation; `new Neo4jStoredQueries(driver)` uses `Neo4jStoredQueries.DefaultDatabase`, `"neo4j"`, the only user database on Community edition |
| `StoredQuery` | `sealed record StoredQuery(Guid Id, Guid VisualizationId, PureQLQuery Query)`: a query as stored, with its id and the id of the visualization it belongs to |

`Neo4jStoredQueries` does not own its driver: whoever creates the `IDriver` disposes it.

## How the operation packages use it

Each operation package exposes statements (a `Neo4j.Driver.Query`: Cypher text and parameters), records that run them on an `IAsyncQueryRunner`, and convenience records over `INeo4jStoredQueries` that open a session on `Driver` against `Database` themselves. Both `IAsyncSession` and `IAsyncTransaction` are `IAsyncQueryRunner`s, so operations from different packages compose inside one transaction:

```csharp
using Neo4j.Driver;
using PureQL.CSharp.Model;
using PureQL.CSharp.Model.Graph.Neo4j.Abstractions;

await using IDriver driver = GraphDatabase.Driver(
    "neo4j://localhost:7687",
    AuthTokens.Basic("neo4j", "password")
);
INeo4jStoredQueries store = new Neo4jStoredQueries(driver);

PureQLQuery query = /* any PureQL query */;
StoredQuery stored = new StoredQuery(Guid.NewGuid(), visualizationId, query);

// One package's convenience record opens the session itself:
await new InsertedQueryOf(store, stored).ApplyAsync();

// Or compose several operations in one transaction:
await using IAsyncSession session = driver.AsyncSession(
    builder => builder.WithDatabase(store.Database)
);
await session.ExecuteWriteAsync(
    async transaction =>
    {
        await new DeletedQueriesOfVisualization(transaction, visualizationId).ApplyAsync();
        await new InsertedQuery(transaction, stored).ApplyAsync();
    }
);
```

`InsertedQueryOf`, `InsertedQuery` and `DeletedQueriesOfVisualization` come from `PureQL.CSharp.Model.Graph.Neo4j.Insertion` and `PureQL.CSharp.Model.Graph.Neo4j.Deletion`; this package itself contains no operation.

## Design Principles

- **Contract only** — no I/O; the records hold their values and nothing else
- **AOT-compatible** — `IsAotCompatible=true`

## Dependencies

- [`PureQL.CSharp.Model`](https://github.com/kudima03/PureQL.CSharp.Model) `0.1.0-preview.14.0.0` — `PureQLQuery`
- [`Neo4j.Driver`](https://www.nuget.org/packages/Neo4j.Driver) `5.28.4` — `IDriver`

## Target Frameworks

- .NET 11

## Installation

```
dotnet add package PureQL.CSharp.Model.Graph.Neo4j.Abstractions
```

## Related packages

- [`PureQL.CSharp.Model.Graph`](https://github.com/kudima03/PureQL.CSharp.Model.Graph) — maps a `PureQLQuery` to a property graph and back, without a driver
- [`PureQL.CSharp.Model.Graph.Neo4j.Creation`](https://github.com/kudima03/PureQL.CSharp.Model.Graph.Neo4j.Creation) — constraints and indexes
- [`PureQL.CSharp.Model.Graph.Neo4j.Insertion`](https://github.com/kudima03/PureQL.CSharp.Model.Graph.Neo4j.Insertion) — writes a query
- [`PureQL.CSharp.Model.Graph.Neo4j.Querying`](https://github.com/kudima03/PureQL.CSharp.Model.Graph.Neo4j.Querying) — reads queries back
- [`PureQL.CSharp.Model.Graph.Neo4j.Deletion`](https://github.com/kudima03/PureQL.CSharp.Model.Graph.Neo4j.Deletion) — deletes queries
- [`PureQL.CSharp.Model.Graph.Neo4j.Updating`](https://github.com/kudima03/PureQL.CSharp.Model.Graph.Neo4j.Updating) — replaces a query in one transaction
- [`PureQL.CSharp.Model.Graph.Neo4j.Existence`](https://github.com/kudima03/PureQL.CSharp.Model.Graph.Neo4j.Existence) — checks the schema and queries exist
- [`PureQL.CSharp.Model.Graph.Neo4j.Destruction`](https://github.com/kudima03/PureQL.CSharp.Model.Graph.Neo4j.Destruction) — drops the schema and every stored query
- [`PureQL.CSharp.Model.Graph.Neo4j.Migration`](https://github.com/kudima03/PureQL.CSharp.Model.Graph.Neo4j.Migration) — migrates stored graphs between model versions
- [`PureQL.CSharp.Model.Serialization`](https://github.com/kudima03/PureQL.CSharp.Model.Serialization) — JSON for the same model
