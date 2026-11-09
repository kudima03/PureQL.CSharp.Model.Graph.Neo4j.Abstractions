namespace PureQL.CSharp.Model.Graph.Neo4j.Abstractions;

/// <summary>
/// A query as stored: its id and the query.
/// </summary>
/// <param name="Id">The id of the stored query.</param>
/// <param name="Query">The query.</param>
public sealed record StoredQuery(Guid Id, PureQLQuery Query);
