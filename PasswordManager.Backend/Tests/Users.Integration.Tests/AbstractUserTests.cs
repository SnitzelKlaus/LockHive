using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PasswordManager.Users.Infrastructure.UserRepository;
using PasswordManager.Users.TestFixtures.Entities;

namespace Users.Integration.Tests;
internal class AbstractUserTests : AbstractEndpointTests
{
    [SetUp]
    public async Task SetUp()
    {
        await CleanDatabase();
        ClearBus();
    }

    private protected async Task CreateAndStoreUserInDb(Guid userId)
    {
        var user = UserEntityFixture.Builder().Id(userId).Build();
        await StoreUserInDb(user);
    }

    private protected async Task StoreUserInDb(UserEntity user)
    {
        var dbContext = GetRequiredService<UserContext>();
        if (dbContext.Users is null)
            throw new InvalidOperationException("Db context not initialized correct");

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();
    }

    private protected async Task<UserEntity> GetUser(Guid userId)
    {
        var dbContext = GetRequiredService<UserContext>();
        if (dbContext.Users is null)
            throw new InvalidOperationException("Db context not initialized correct");

        return await dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
    }

    private async Task CleanDatabase()
    {
        var dbContext = GetRequiredService<UserContext>();
        if (dbContext.Users is null)
            throw new InvalidOperationException("Db context not initialized correct");

        dbContext.Users.RemoveRange(dbContext.Users);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();
    }
}
