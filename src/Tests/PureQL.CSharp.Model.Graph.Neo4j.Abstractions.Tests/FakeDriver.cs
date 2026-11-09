using Neo4j.Driver;

namespace PureQL.CSharp.Model.Graph.Neo4j.Abstractions.Tests;

internal sealed class FakeDriver : IDriver
{
    public Config Config => throw new NotSupportedException();

    public bool Encrypted => throw new NotSupportedException();

    public IAsyncSession AsyncSession()
    {
        throw new NotSupportedException();
    }

    public IAsyncSession AsyncSession(Action<SessionConfigBuilder> action)
    {
        throw new NotSupportedException();
    }

    public Task CloseAsync()
    {
        throw new NotSupportedException();
    }

    public Task<IServerInfo> GetServerInfoAsync()
    {
        throw new NotSupportedException();
    }

    public Task<bool> TryVerifyConnectivityAsync()
    {
        throw new NotSupportedException();
    }

    public Task VerifyConnectivityAsync()
    {
        throw new NotSupportedException();
    }

    public Task<bool> SupportsMultiDbAsync()
    {
        throw new NotSupportedException();
    }

    public Task<bool> SupportsSessionAuthAsync()
    {
        throw new NotSupportedException();
    }

    public IExecutableQuery<IRecord, IRecord> ExecutableQuery(string cypher)
    {
        throw new NotSupportedException();
    }

    public IBookmarkManager GetExecutableQueryBookmarkManager()
    {
        throw new NotSupportedException();
    }

    public Task<bool> VerifyAuthenticationAsync(IAuthToken authToken)
    {
        throw new NotSupportedException();
    }

    public void Dispose() { }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}
