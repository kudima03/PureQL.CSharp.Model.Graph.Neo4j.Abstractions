using Neo4j.Driver;

namespace PureQL.CSharp.Model.Graph.Neo4j.Abstractions;

/// <summary>A Neo4j database holding PureQL query graphs.</summary>
/// <remarks>
/// The operation packages open their sessions on <see cref="Driver"/> against
/// <see cref="Database"/>, and run their statements on an
/// <see cref="IAsyncQueryRunner"/> so that several operations compose inside one
/// transaction.
/// </remarks>
public interface INeo4jStoredQueries
{
    /// <summary>The driver connected to the Neo4j server.</summary>
    public IDriver Driver { get; }

    /// <summary>
    /// The name of the database holding the query graphs: <c>neo4j</c> on Community
    /// edition, which has a single user database.
    /// </summary>
    public string Database { get; }
}
