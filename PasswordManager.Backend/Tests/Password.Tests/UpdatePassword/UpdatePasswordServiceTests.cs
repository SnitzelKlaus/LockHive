using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Password.Messages.UpdatePassword;
using PasswordManager.Password.ApplicationServices.Operations;
using PasswordManager.Password.ApplicationServices.Password.UpdatePassword;
using PasswordManager.Password.ApplicationServices.Repositories.Password;
using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.Domain.Password;
using PasswordManager.Password.TestFixtures.Operations;
using PasswordManager.Password.TestFixtures.Password;
using Rebus.Bus;

namespace PasswordManager.Password.Tests.UpdatePassword;
internal sealed class UpdatePasswordServiceTests
{
    private Mock<IOperationService> _operationServiceMock;
    private Mock<IBus> _busMock;
    private Mock<ILogger<UpdatePasswordService>> _loggerMock;
    private Mock<IPasswordRepository> _passwordRepositoryMock;
    private IUpdatePasswordService _updatePasswordService;

    private static Guid _passwordId = Guid.NewGuid();
    private const string _createdBy = "createdBy";

    [SetUp]
    public void Setup()
    {
        _operationServiceMock = new Mock<IOperationService>();
        _busMock = new Mock<IBus>();
        _loggerMock = new Mock<ILogger<UpdatePasswordService>>();
        _passwordRepositoryMock = new Mock<IPasswordRepository>();

        _updatePasswordService = new UpdatePasswordService(_operationServiceMock.Object, _busMock.Object, _loggerMock.Object, _passwordRepositoryMock.Object);
    }
    #region Request update password

    [Test]
    public async Task RequestUpdatePassword_ReturnsAcceptedResult_HappyPath()
    {
        var passwordModel = PasswordModelFixture.Builder().WithId(_passwordId).Build();
        var operation = OperationFixture.Builder().WithName(OperationName.UpdatePassword).Build();
        var operationDetails = new OperationDetails(_createdBy);

        _passwordRepositoryMock.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(passwordModel);

        _operationServiceMock.Setup(operation => operation.QueueOperation(It.IsAny<Operation>())).ReturnsAsync(operation);

        var requestUpdatePasswordOperationResult = await _updatePasswordService.RequestUpdatePassword(passwordModel, operationDetails);

        Assert.That(requestUpdatePasswordOperationResult, Is.Not.Null);
        Assert.That(requestUpdatePasswordOperationResult.Status, Is.EqualTo(OperationResultStatus.Accepted));

        _passwordRepositoryMock.Verify(repo => repo.Get(It.Is<Guid>(id => id == _passwordId)));
        _operationServiceMock.Verify(operation => operation.QueueOperation(It.Is<Operation>(ops =>
                                    ops.PasswordId == _passwordId
                                    && ops.Data.ContainsKey("newPasswordUrl")
                                    && ops.Data["newPasswordUrl"] == passwordModel.Url
                                    && ops.CreatedBy == _createdBy
                                    && ops.Name == OperationName.UpdatePassword
                                    )));
        _busMock.Verify(bus => bus.Send(It.Is<UpdatePasswordCommand>(msq => msq.RequestId == operation.RequestId), null));
    }

    [Test]
    public async Task RequestUpdatePassword_QueuesOperation()
    {
        var passwordModel = PasswordModelFixture.Builder().WithId(_passwordId).Build();
        var operation = OperationFixture.Builder().WithName(OperationName.UpdatePassword).Build();
        var operationDetails = new OperationDetails(_createdBy);

        _passwordRepositoryMock.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(passwordModel);

        _operationServiceMock.Setup(operation => operation.QueueOperation(It.IsAny<Operation>())).ReturnsAsync(operation);

        var requestUpdatePasswordOperationResult = await _updatePasswordService.RequestUpdatePassword(passwordModel, operationDetails);

        _operationServiceMock.Verify(operation => operation.QueueOperation(It.Is<Operation>(ops =>
                                    ops.PasswordId == _passwordId
                                    && ops.Data.ContainsKey("newPasswordUrl")
                                    && ops.Data["newPasswordUrl"] == passwordModel.Url
                                    && ops.CreatedBy == _createdBy
                                    && ops.Name == OperationName.UpdatePassword
        )));
    }

    [Test]
    public async Task RequestUpdatePassword_SendsCommand()
    {
        var passwordModel = PasswordModelFixture.Builder().WithId(_passwordId).Build();
        var operation = OperationFixture.Builder().WithName(OperationName.UpdatePassword).Build();
        var operationDetails = new OperationDetails(_createdBy);

        _passwordRepositoryMock.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(passwordModel);

        _operationServiceMock.Setup(operation => operation.QueueOperation(It.IsAny<Operation>())).ReturnsAsync(operation);

        var requestUpdatePasswordOperationResult = await _updatePasswordService.RequestUpdatePassword(passwordModel, operationDetails);

        _busMock.Verify(bus => bus.Send(It.Is<UpdatePasswordCommand>(msq => msq.RequestId == operation.RequestId), null));
    }


    [Test]
    public async Task RequestUpdatePassword_ReturnsInvalidOperationsResult_WhenPasswordNotFound()
    {
        var passwordModel = PasswordModelFixture.Builder().WithId(_passwordId).Build();
        var operation = OperationFixture.Builder().WithName(OperationName.UpdatePassword).Build();
        var operationDetails = new OperationDetails(_createdBy);

        _passwordRepositoryMock.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync((PasswordModel)null);

        var requestUpdatePasswordOperationResult = await _updatePasswordService.RequestUpdatePassword(passwordModel, operationDetails);

        Assert.That(requestUpdatePasswordOperationResult, Is.Not.Null);
        Assert.That(requestUpdatePasswordOperationResult.Status, Is.EqualTo(OperationResultStatus.InvalidOperationRequest));

        _passwordRepositoryMock.Verify(repo => repo.Get(It.Is<Guid>(id => id == _passwordId)));
        _operationServiceMock.Verify(operation => operation.QueueOperation(It.Is<Operation>(ops =>
                                    ops.PasswordId == _passwordId
                                    && ops.Data.ContainsKey("newPasswordUrl")
                                    && ops.Data["newPasswordUrl"] == passwordModel.Url
                                    && ops.CreatedBy == _createdBy
                                    && ops.Name == OperationName.UpdatePassword
                                    )), Times.Never);
        _busMock.Verify(bus => bus.Send(It.Is<UpdatePasswordCommand>(msq => msq.RequestId == operation.RequestId), null), Times.Never);
    }

    [Test]
    public async Task RequestUpdatePassword_ReturnsInvalidOperationsResult_WhenPasswordIsMarkedAsDeleted()
    {
        var passwordModel = PasswordModelFixture.Builder().WithId(_passwordId).IsDeleted().Build();
        var operation = OperationFixture.Builder().WithName(OperationName.UpdatePassword).Build();
        var operationDetails = new OperationDetails(_createdBy);

        _passwordRepositoryMock.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(passwordModel);

        var requestUpdatePasswordOperationResult = await _updatePasswordService.RequestUpdatePassword(passwordModel, operationDetails);

        Assert.That(requestUpdatePasswordOperationResult, Is.Not.Null);
        Assert.That(requestUpdatePasswordOperationResult.Status, Is.EqualTo(OperationResultStatus.InvalidOperationRequest));

        _passwordRepositoryMock.Verify(repo => repo.Get(It.Is<Guid>(id => id == _passwordId)));
        _operationServiceMock.Verify(operation => operation.QueueOperation(It.Is<Operation>(ops =>
                                    ops.PasswordId == _passwordId
                                    && ops.Data.ContainsKey("newPasswordUrl")
                                    && ops.Data["newPasswordUrl"] == passwordModel.Url
                                    && ops.CreatedBy == _createdBy
                                    && ops.Name == OperationName.UpdatePassword
                                    )), Times.Never);
        _busMock.Verify(bus => bus.Send(It.Is<UpdatePasswordCommand>(msq => msq.RequestId == operation.RequestId), null), Times.Never);
    }
    #endregion

    #region Update password
    [Test]
    public async Task UpdatePassword_UpdatesPassword_WhenCallingRepository()
    {
        var passwordModel = PasswordModelFixture.Builder().WithId(_passwordId).Build();

        await _updatePasswordService.UpdatePassword(passwordModel);

        _passwordRepositoryMock.Verify(repo => repo.UpdatePassword(It.Is<PasswordModel>(model => model.Id == _passwordId
            && model.Url == passwordModel.Url
            && model.FriendlyName == passwordModel.FriendlyName
            && model.Username == passwordModel.Username
            && model.Password == passwordModel.Password
            )));
    }

    [Test]
    public void UpdatePassword_ThrowsException_WhenAnErrorOccuredUpdatingPassword()
    {
        var passwordModel = PasswordModelFixture.Builder().WithId(_passwordId).Build();

        _passwordRepositoryMock.Setup(repo => repo.UpdatePassword(It.IsAny<PasswordModel>())).ThrowsAsync(new PasswordRepositoryException("Error updating password"));

        Assert.ThrowsAsync<PasswordRepositoryException>(() => _updatePasswordService.UpdatePassword(passwordModel));
    }
    #endregion
}
