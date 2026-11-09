namespace PureQL.CSharp.Model.Graph.Neo4j.Abstractions;

/// <summary>
/// A query as stored: its id, the visualization it belongs to, and the query.
/// </summary>
/// <param name="Id">The id of the stored query.</param>
/// <param name="VisualizationId">
/// The id of the visualization the query belongs to.
/// </param>
/// <param name="Query">The query.</param>
public sealed record StoredQuery(Guid Id, Guid VisualizationId, PureQLQuery Query);
