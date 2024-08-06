using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Moq;
using NUnit.Framework;
using Password.Messages.UpdatePassword;
using Password.Worker.Service.UpdatePassword;
using PasswordManager.Password.ApplicationServices.Operations;
using PasswordManager.Password.ApplicationServices.Password.UpdatePassword;
using PasswordManager.Password.ApplicationServices.Repositories.Password;
using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.Domain.Password;
using PasswordManager.Password.TestFixtures.Operations;
using PasswordManager.Password.TestFixtures.Password;
using Rebus.Bus;

namespace PasswordManager.Password.Tests.UpdatePassword;
internal sealed class UpdatePasswordCommandHandlerTests
{
    private Mock<IUpdatePasswordService> _updatePasswordServiceMock;
    private Mock<IOperationService> _operationServiceMock;
    private Mock<ILogger<UpdatePasswordCommandHandler>> _loggerMock;
    private Mock<IBus> _busMock;

    private UpdatePasswordCommandHandler _updatePasswordCommandHandler;
    private const string _requestId = "request-id";
    private static Guid _passwordId = Guid.NewGuid();
    private const OperationName _operationName = OperationName.UpdatePassword;

    [SetUp]
    public void Setup()
    {
        _updatePasswordServiceMock = new Mock<IUpdatePasswordService>();
        _operationServiceMock = new Mock<IOperationService>();
        _loggerMock = new Mock<ILogger<UpdatePasswordCommandHandler>>();
        _busMock = new Mock<IBus>();
        _updatePasswordCommandHandler = new UpdatePasswordCommandHandler(_updatePasswordServiceMock.Object, _operationServiceMock.Object, _loggerMock.Object, _busMock.Object);
    }

    [Test]
    public async Task Handle_UpdatePasswordCommand()
    {
        var passwordModel = PasswordModelFixture.Builder().WithId(_passwordId).Build();

        var updatePasswordCommand = new UpdatePasswordCommand(_requestId);
        var operation = OperationFixture.Builder().WithRequestId(_requestId).WithPasswordId(_passwordId).WithName(_operationName)
            .WithAddData(OperationDataConstants.NewPasswordUrl, passwordModel.Url)
            .WithAddData(OperationDataConstants.NewPasswordFriendlyName, passwordModel.FriendlyName)
            .WithAddData(OperationDataConstants.NewPasswordUsername, passwordModel.Username)
            .WithAddData(OperationDataConstants.NewPasswordPassword, passwordModel.Password)
            .Build();

        _operationServiceMock.Setup(operation => operation.GetOperationByRequestId(It.IsAny<string>())).ReturnsAsync(operation);

        await _updatePasswordCommandHandler.Handle(updatePasswordCommand);

        _operationServiceMock.Verify(operation => operation.GetOperationByRequestId(It.Is<string>(id => id == _requestId)));
        _operationServiceMock.Verify(operation => operation.UpdateOperationStatus(It.Is<string>(id => id == _requestId), It.Is<OperationStatus>(operationStatus => operationStatus == OperationStatus.Processing)));
        _operationServiceMock.Verify(operation => operation.UpdateOperationStatus(It.Is<string>(id => id == _requestId), It.Is<OperationStatus>(operationStatus => operationStatus == OperationStatus.Completed)));
        _updatePasswordServiceMock.Verify(service => service.UpdatePassword(It.Is<PasswordModel>(id => id.Id == _passwordId)));
        _busMock.Verify(bus => bus.Publish(It.Is<UpdatePasswordEvent>(msg => msg.PasswordId == _passwordId && msg.RequestId == _requestId), null));
    }

    [Test]
    public async Task Handle_ThrowsInvalidOperationException_WhenOperationNotFound()
    {
        var updatePasswordCommand = new UpdatePasswordCommand(_requestId);  

        _operationServiceMock.Setup(operation => operation.GetOperationByRequestId(It.IsAny<string>())).ReturnsAsync((Operation)null);

        Assert.ThrowsAsync<InvalidOperationException>(() => _updatePasswordCommandHandler.Handle(updatePasswordCommand));
    }

    [Test]
    public async Task Handle_ThrowsPasswordRepositoryExceptionAndUpdatesOperationToFailed_WhenPasswordRepositoryExceptionIsCaught()
    {
        var passwordModel = PasswordModelFixture.Builder().WithId(_passwordId).Build();
        var updatePasswordCommand = new UpdatePasswordCommand(_requestId);
        
        var operation = OperationFixture.Builder().WithRequestId(_requestId).WithPasswordId(_passwordId).WithName(_operationName)
            .WithAddData(OperationDataConstants.NewPasswordUrl, passwordModel.Url)
            .WithAddData(OperationDataConstants.NewPasswordFriendlyName, passwordModel.FriendlyName)
            .WithAddData(OperationDataConstants.NewPasswordUsername, passwordModel.Username)
            .WithAddData(OperationDataConstants.NewPasswordPassword, passwordModel.Password)
            .Build();

        _operationServiceMock.Setup(operation => operation.GetOperationByRequestId(It.IsAny<string>())).ReturnsAsync(operation);
        _updatePasswordServiceMock.Setup(service => service.UpdatePassword(It.IsAny<PasswordModel>())).ThrowsAsync(new PasswordRepositoryException("An error occurred updating password"));

        Assert.ThrowsAsync<PasswordRepositoryException>(() => _updatePasswordCommandHandler.Handle(updatePasswordCommand));
        _operationServiceMock.Verify(operation => operation.UpdateOperationStatus(It.Is<string>(id => id == _requestId), It.Is<OperationStatus>(operationStatus => operationStatus == OperationStatus.Failed)));
        _operationServiceMock.Verify(operation => operation.UpdateOperationStatus(It.Is<string>(id => id == _requestId), It.Is<OperationStatus>(operationStatus => operationStatus == OperationStatus.Processing)));
        _busMock.Verify(bus => bus.Publish(It.Is<UpdatePasswordFailedEvent>(msg => msg.PasswordId == _passwordId && msg.RequestId == _requestId), null));
    }
}
