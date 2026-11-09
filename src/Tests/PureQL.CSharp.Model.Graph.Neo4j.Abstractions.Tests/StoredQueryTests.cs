using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Graph.Neo4j.Abstractions.Tests;

public sealed record StoredQueryTests
{
    private static readonly Guid Id = new Guid("5d1f3c2a-6b7e-4f80-9a1b-2c3d4e5f6a7b");

    private static PureQLQuery Query()
    {
        return Query("status");
    }

    private static PureQLQuery Query(string field)
    {
        return new MainPlainQueryWithoutPagination(
            from: new FromEntity("shop.orders"),
            select:
            [
                new SelectItemProjection(field, new FieldString("shop.orders", field)),
            ]
        );
    }

    [Fact]
    public void KeepsId()
    {
        Assert.Equal(Id, new StoredQuery(Id, Query()).Id);
    }

    [Fact]
    public void KeepsQuery()
    {
        PureQLQuery query = Query();
        Assert.Same(query.Value, new StoredQuery(Id, query).Query.Value);
    }

    [Fact]
    public void EqualsWithSameValues()
    {
        PureQLQuery query = Query();
        Assert.Equal(new StoredQuery(Id, query), new StoredQuery(Id, query));
    }

    [Fact]
    public void HashCodeEqualsWithSameValues()
    {
        PureQLQuery query = Query();
        Assert.Equal(
            new StoredQuery(Id, query).GetHashCode(),
            new StoredQuery(Id, query).GetHashCode()
        );
    }

    [Fact]
    public void DiffersWithOtherId()
    {
        PureQLQuery query = Query();
        Assert.NotEqual(
            new StoredQuery(Id, query),
            new StoredQuery(Guid.NewGuid(), query)
        );
    }

    [Fact]
    public void DiffersWithOtherQuery()
    {
        Assert.NotEqual(
            new StoredQuery(Id, Query("status")),
            new StoredQuery(Id, Query("total"))
        );
    }

    [Fact]
    public void ReplacesQueryWithExpression()
    {
        StoredQuery stored = new StoredQuery(Id, Query("status"));
        PureQLQuery other = Query("total");
        Assert.Equal(new StoredQuery(Id, other), stored with { Query = other });
    }
}
