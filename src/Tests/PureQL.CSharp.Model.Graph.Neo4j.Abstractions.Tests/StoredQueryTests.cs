using PureQL.CSharp.Model.Fields;

namespace PureQL.CSharp.Model.Graph.Neo4j.Abstractions.Tests;

public sealed record StoredQueryTests
{
    private static readonly Guid Id = new Guid("5d1f3c2a-6b7e-4f80-9a1b-2c3d4e5f6a7b");

    private static readonly Guid VisualizationId = new Guid(
        "0e9d8c7b-6a5f-4e3d-8c2b-1a0f9e8d7c6b"
    );

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
        Assert.Equal(Id, new StoredQuery(Id, VisualizationId, Query()).Id);
    }

    [Fact]
    public void KeepsVisualizationId()
    {
        Assert.Equal(
            VisualizationId,
            new StoredQuery(Id, VisualizationId, Query()).VisualizationId
        );
    }

    [Fact]
    public void KeepsQuery()
    {
        PureQLQuery query = Query();
        Assert.Same(query.Value, new StoredQuery(Id, VisualizationId, query).Query.Value);
    }

    [Fact]
    public void EqualsWithSameValues()
    {
        PureQLQuery query = Query();
        Assert.Equal(
            new StoredQuery(Id, VisualizationId, query),
            new StoredQuery(Id, VisualizationId, query)
        );
    }

    [Fact]
    public void HashCodeEqualsWithSameValues()
    {
        PureQLQuery query = Query();
        Assert.Equal(
            new StoredQuery(Id, VisualizationId, query).GetHashCode(),
            new StoredQuery(Id, VisualizationId, query).GetHashCode()
        );
    }

    [Fact]
    public void DiffersWithOtherId()
    {
        PureQLQuery query = Query();
        Assert.NotEqual(
            new StoredQuery(Id, VisualizationId, query),
            new StoredQuery(Guid.NewGuid(), VisualizationId, query)
        );
    }

    [Fact]
    public void DiffersWithOtherVisualizationId()
    {
        PureQLQuery query = Query();
        Assert.NotEqual(
            new StoredQuery(Id, VisualizationId, query),
            new StoredQuery(Id, Guid.NewGuid(), query)
        );
    }

    [Fact]
    public void DiffersWithOtherQuery()
    {
        Assert.NotEqual(
            new StoredQuery(Id, VisualizationId, Query("status")),
            new StoredQuery(Id, VisualizationId, Query("total"))
        );
    }

    [Fact]
    public void MovesToOtherVisualizationWithExpression()
    {
        StoredQuery stored = new StoredQuery(Id, VisualizationId, Query());
        Guid other = Guid.NewGuid();
        Assert.Equal(
            new StoredQuery(Id, other, stored.Query),
            stored with
            {
                VisualizationId = other,
            }
        );
    }
}
