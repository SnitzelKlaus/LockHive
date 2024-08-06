using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using NUnit.Framework;
using System.Diagnostics;
using System.Reflection;
using Testcontainers.MsSql;

namespace Users.Integration.Tests;
[SetUpFixture]
public class IntegrationTestAssemblyFixtureSetup
{
    private const int MsSqlPrivatePort = 1433;
    private static readonly MemoryStream OutputStream = new();
    private static readonly MemoryStream ErrorOutputStream = new();

    public static ushort MappedPort;
    private MsSqlContainer _msSqlServerContainer;

    [OneTimeSetUp]
    public async Task Setup()
    {
        await Console.Error.WriteLineAsync("Starting up docker images (mssql for now)");
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        CreateSqlServerContainer();
        await _msSqlServerContainer.StartAsync();
        stopwatch.Stop();
        MappedPort = _msSqlServerContainer.GetMappedPublicPort(MsSqlPrivatePort);
        await Console.Error.WriteLineAsync(
            $"Fixtures setup done - using port {MappedPort} - took {stopwatch.ElapsedMilliseconds} ms");
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _msSqlServerContainer.DisposeAsync();
        await Console.Out.WriteLineAsync("test one time tear down");
    }

    private void CreateSqlServerContainer()
    {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var workingPath = Path.GetDirectoryName(assemblyLocation);
        if (workingPath is null) throw new InvalidOperationException("could not get working path of running dll");

        var dockerEntryPoints = Path.Combine(workingPath, "scripts", "docker-entrypoint.sh").Replace('\\', '/');
        var dockerDbInit = Path.Combine(workingPath, "scripts", "docker-db-init.sh").Replace('\\', '/');
        var dockerDbSql = Path.Combine(workingPath, "scripts", "docker-db-init-passwordManager-users.sql")
            .Replace('\\', '/');


        _msSqlServerContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithName("passwordManager-users-integration-test-db")
            .WithCommand("/bin/bash", "./entrypoint.sh")
            .WithBindMount(dockerEntryPoints, "/entrypoint.sh", AccessMode.ReadOnly)
            .WithBindMount(dockerDbInit, "/db-init.sh", AccessMode.ReadOnly)
            .WithBindMount(dockerDbSql, "/db-init-passwordManager-users.sql", AccessMode.ReadOnly)
            .WithPortBinding(MsSqlPrivatePort, true)
            .WithEnvironment("ACCEPT_EULA", "Y").WithEnvironment("SA_PASSWORD", "int7estP@ss")
            .WithEnvironment("MSSQL_PID", "Express")
            .WithOutputConsumer(Consume.RedirectStdoutAndStderrToStream(OutputStream, ErrorOutputStream))
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("passwordmanager users db set up"))
            .Build();
    }
}
