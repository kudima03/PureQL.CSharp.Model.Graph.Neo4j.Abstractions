# Changelog

All notable changes to `PureQL.CSharp.Model.Graph.Neo4j.Abstractions` are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.1.0-preview.1.1.0] - 2026-11-11

Moves to the .NET 11 release of the model.

### Changed

- Depends on `PureQL.CSharp.Model` `0.1.0-preview.14.1.0` (was
  `0.1.0-preview.14.0.0`).

## [0.1.0-preview.1.0.0] - 2026-11-09

A stored query no longer knows the visualization it is shown in:
visualizations hold the query id instead.

### Changed

- **Breaking:** **`StoredQuery`** is now `sealed record StoredQuery(Guid Id,
  PureQLQuery Query)`. The `VisualizationId` property is removed, along with
  the three-argument constructor and `Deconstruct`.

## [0.1.0-preview.0.1.0] - 2026-11-09

First release: the Neo4j stored-queries contract shared by the Neo4j operation
packages of the `PureQL.CSharp.Model.Graph` family.

### Added

- **`INeo4jStoredQueries`** — the Neo4j database holding PureQL query graphs:
  the `IDriver Driver` connected to the server and the `string Database` name.
- **`Neo4jStoredQueries`** — `sealed record Neo4jStoredQueries(IDriver Driver,
  string Database)`; `new Neo4jStoredQueries(driver)` uses
  `Neo4jStoredQueries.DefaultDatabase` (`"neo4j"`). The record does not own
  its driver.
- **`StoredQuery`** — `sealed record StoredQuery(Guid Id, Guid VisualizationId,
  PureQLQuery Query)`: a query as stored.
- Depends on `PureQL.CSharp.Model` `0.1.0-preview.14.0.0` and `Neo4j.Driver`
  `6.5.1`; targets .NET 11 and is AOT-compatible.
