using Neo4j.Driver;

namespace PureQL.CSharp.Model.Graph.Neo4j.Abstractions;

/// <summary>A Neo4j database holding PureQL query graphs.</summary>
/// <param name="Driver">The driver connected to the Neo4j server.</param>
/// <param name="Database">The name of the database holding the query graphs.</param>
/// <remarks>
/// The record does not own <paramref name="Driver"/>: whoever created the driver
/// disposes it.
/// </remarks>
public sealed record Neo4jStoredQueries(IDriver Driver, string Database)
    : INeo4jStoredQueries
{
    /// <summary>
    /// The default database of a Neo4j server, <c>neo4j</c>, which is the only user
    /// database on Community edition.
    /// </summary>
    public const string DefaultDatabase = "neo4j";

    /// <summary>
    /// Creates the stored queries of the server's default database,
    /// <see cref="DefaultDatabase"/>.
    /// </summary>
    /// <param name="driver">The driver connected to the Neo4j server.</param>
    public Neo4jStoredQueries(IDriver driver)
        : this(driver, DefaultDatabase) { }
}
