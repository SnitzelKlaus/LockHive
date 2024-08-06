using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Users.Integration.Tests;
internal abstract class AbstractEndpointTests
{
    private HttpClient _client;
    private BaseApiServiceWebApplicationFactory _factory;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _factory = new BaseApiServiceWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OnTimeTearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    private BaseApiServiceWebApplicationFactory GetApplicationFactory()
    {
        if (_factory == null) throw new InvalidOperationException("Could not initialize app context");
        return _factory;
    }

    protected HttpClient GetHttpClient()
    {
        if (_client == null) throw new InvalidOperationException("Could not initialize app context");
        return _client;
    }

    protected T GetRequiredService<T>() where T : notnull
    {
        var scope = GetApplicationFactory().Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();
        return service;
    }

    protected void ClearBus()
    {
        var fakeBus = GetApplicationFactory().GetUsedBus();
        fakeBus.Clear();
    }
}
