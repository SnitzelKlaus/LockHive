using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.TestFixtures.Operations;

namespace PasswordManager.Users.Tests.CreateUserPassword;

public class CreateUserPasswordOperationHelperTests
{
    private static Guid _userId = Guid.NewGuid();
    public void Map_ReturnsUserPasswordModel_OnHappyPath()
    {
        var operationFixture = OperationFixture.Builder().WithUserId(_userId)
           .WithAddData(OperationDataConstants.CreateUserPasswordUrl, "UserPasswordUrl")
           .WithAddData(OperationDataConstants.CreateUserPasswordFriendlyName, "UserPasswordFriendlyName")
           .WithAddData(OperationDataConstants.CreateUserPasswordUsername, "UserPasswordUsername")
           .WithAddData(OperationDataConstants.CreateUserPasswordPassword, "UserPasswordPassword")
           .Build();
    }
}
