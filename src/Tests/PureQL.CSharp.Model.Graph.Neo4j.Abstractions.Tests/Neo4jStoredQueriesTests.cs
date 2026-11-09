using Neo4j.Driver;

namespace PureQL.CSharp.Model.Graph.Neo4j.Abstractions.Tests;

public sealed record Neo4jStoredQueriesTests
{
    [Fact]
    public void KeepsDriver()
    {
        IDriver driver = new FakeDriver();
        Assert.Same(driver, new Neo4jStoredQueries(driver, "queries").Driver);
    }

    [Fact]
    public void KeepsDatabase()
    {
        Assert.Equal(
            "queries",
            new Neo4jStoredQueries(new FakeDriver(), "queries").Database
        );
    }

    [Fact]
    public void DefaultDatabaseIsNeo4j()
    {
        Assert.Equal("neo4j", Neo4jStoredQueries.DefaultDatabase);
    }

    [Fact]
    public void UsesDefaultDatabaseWithoutDatabase()
    {
        Assert.Equal(
            Neo4jStoredQueries.DefaultDatabase,
            new Neo4jStoredQueries(new FakeDriver()).Database
        );
    }

    [Fact]
    public void KeepsDriverWithoutDatabase()
    {
        IDriver driver = new FakeDriver();
        Assert.Same(driver, new Neo4jStoredQueries(driver).Driver);
    }

    [Fact]
    public void EqualsWithDefaultDatabaseNamed()
    {
        IDriver driver = new FakeDriver();
        Assert.Equal(
            new Neo4jStoredQueries(driver, "neo4j"),
            new Neo4jStoredQueries(driver)
        );
    }

    [Fact]
    public void EqualsWithSameDriverAndDatabase()
    {
        IDriver driver = new FakeDriver();
        Assert.Equal(
            new Neo4jStoredQueries(driver, "queries"),
            new Neo4jStoredQueries(driver, "queries")
        );
    }

    [Fact]
    public void HashCodeEqualsWithSameDriverAndDatabase()
    {
        IDriver driver = new FakeDriver();
        Assert.Equal(
            new Neo4jStoredQueries(driver, "queries").GetHashCode(),
            new Neo4jStoredQueries(driver, "queries").GetHashCode()
        );
    }

    [Fact]
    public void DiffersWithOtherDatabase()
    {
        IDriver driver = new FakeDriver();
        Assert.NotEqual(
            new Neo4jStoredQueries(driver, "queries"),
            new Neo4jStoredQueries(driver, "archive")
        );
    }

    [Fact]
    public void DiffersWithOtherDriver()
    {
        Assert.NotEqual(
            new Neo4jStoredQueries(new FakeDriver(), "queries"),
            new Neo4jStoredQueries(new FakeDriver(), "queries")
        );
    }

    [Fact]
    public void ExposesDriverThroughInterface()
    {
        IDriver driver = new FakeDriver();
        INeo4jStoredQueries store = new Neo4jStoredQueries(driver);
        Assert.Same(driver, store.Driver);
    }

    [Fact]
    public void ExposesDatabaseThroughInterface()
    {
        INeo4jStoredQueries store = new Neo4jStoredQueries(new FakeDriver(), "queries");
        Assert.Equal("queries", store.Database);
    }

    [Fact]
    public void ChangesDatabaseWithExpression()
    {
        Neo4jStoredQueries store = new Neo4jStoredQueries(new FakeDriver());
        Assert.Equal("queries", (store with { Database = "queries" }).Database);
    }
}
