using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PasswordManager.Users.ApplicationServices.Operations;
using PasswordManager.Users.ApplicationServices.UserPassword.CreateUserPassword;
using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.Domain.User;
using PasswordManager.Users.TestFixtures.Operations;
using Rebus.Bus;
using Users.Messages.CreateUserPassword;
using Users.Worker.Service.CreateUserPassword;

namespace PasswordManager.Users.Tests.CreateUserPassword;
internal class CreateUserPasswordCommandHandlerTests
{
    private Mock<ICreateUserPasswordService> _createUserPasswordServiceMock;
    private Mock<IOperationService> _operationServiceMock;
    private Mock<ILogger<CreateUserPasswordCommandHandler>> _loggerMock;
    private Mock<IBus> _busMock;

    private CreateUserPasswordCommandHandler _createUserPasswordCommandHandler;
    private readonly string _requestId = Guid.NewGuid().ToString();
    private static Guid _userId = Guid.NewGuid();

    [SetUp]
    public void Setup()
    {
        _createUserPasswordServiceMock = new Mock<ICreateUserPasswordService>();
        _operationServiceMock = new Mock<IOperationService>();
        _loggerMock = new Mock<ILogger<CreateUserPasswordCommandHandler>>();
        _busMock = new Mock<IBus>();

        _createUserPasswordCommandHandler = new CreateUserPasswordCommandHandler(_createUserPasswordServiceMock.Object, _operationServiceMock.Object, _loggerMock.Object, _busMock.Object);
    }

    [Test]
    public async Task Handle_PublishEventAndMarkOperationAsCompleted_OnHappyPath()
    {
        var createUserPasswordCommand = new CreateUserPasswordCommand(_requestId);

        var operationFixture = OperationFixture.Builder().WithUserId(_userId).WithRequestId(_requestId).WithStatus(OperationStatus.Queued)
            .WithAddData(OperationDataConstants.CreateUserPasswordUrl, "UserPasswordUrl")
            .WithAddData(OperationDataConstants.CreateUserPasswordFriendlyName, "UserPasswordFriendlyName")
            .WithAddData(OperationDataConstants.CreateUserPasswordUsername, "UserPasswordUsername")
            .WithAddData(OperationDataConstants.CreateUserPasswordPassword, "UserPasswordPassword")
            .Build();

        _operationServiceMock.Setup(operation => operation.GetOperationByRequestId(It.IsAny<string>())).ReturnsAsync(operationFixture);

        await _createUserPasswordCommandHandler.Handle(createUserPasswordCommand);

        _operationServiceMock.Verify(operation => operation.GetOperationByRequestId(It.Is<string>(requestId => requestId == _requestId)));
        _operationServiceMock.Verify(operation => operation.UpdateOperationStatus(It.Is<string>(requestId => requestId == _requestId), OperationStatus.Processing));

        _createUserPasswordServiceMock.Verify(service => service.CreateUserPassword(It.IsAny<UserPasswordModel>()));

        _operationServiceMock.Verify(operation => operation.UpdateOperationStatus(It.Is<string>(requestId => requestId == _requestId), It.Is<OperationStatus>(status => status == OperationStatus.Completed)));

        _busMock.Verify(bus => bus.Publish(It.Is<CreateUserPasswordEvent>(createUserPasswordEvent => createUserPasswordEvent.UserId == _userId
        && createUserPasswordEvent.RequestId == _requestId), null));
    }

    [Test]
    public void Handle_ThrowsException_WhenOperationIsNotFound()
    {
        var createUserPasswordCommand = new CreateUserPasswordCommand(_requestId);

        _operationServiceMock.Setup(operation => operation.GetOperationByRequestId(It.IsAny<string>())).ReturnsAsync((Operation)null);

        Assert.ThrowsAsync<InvalidOperationException>(() => _createUserPasswordCommandHandler.Handle(createUserPasswordCommand));
    }

    [Test]
    public async Task Handle_PublishFailedEventAndMarkOperationAsFailed_WhenCreateUserPasswordServiceExceptionIsCaught()
    {
        var createUserPasswordCommand = new CreateUserPasswordCommand(_requestId);
        var operationFixture = OperationFixture.Builder().WithUserId(_userId).WithRequestId(_requestId).WithStatus(OperationStatus.Queued)
            .WithAddData(OperationDataConstants.CreateUserPasswordUrl, "UserPasswordUrl")
            .WithAddData(OperationDataConstants.CreateUserPasswordFriendlyName, "UserPasswordFriendlyName")
            .WithAddData(OperationDataConstants.CreateUserPasswordUsername, "UserPasswordUsername")
            .WithAddData(OperationDataConstants.CreateUserPasswordPassword, "UserPasswordPassword")
            .Build();

        _operationServiceMock.Setup(operation => operation.GetOperationByRequestId(It.IsAny<string>())).ReturnsAsync(operationFixture);
        _createUserPasswordServiceMock.Setup(service => service.CreateUserPassword(It.IsAny<UserPasswordModel>()))
            .ThrowsAsync(new CreateUserPasswordServiceException("Cannot create user password", new Exception()));

        await _createUserPasswordCommandHandler.Handle(createUserPasswordCommand);

        _operationServiceMock.Verify(operation => operation.UpdateOperationStatus(It.Is<string>(requestId => requestId == _requestId),
            It.Is<OperationStatus>(status => status == OperationStatus.Failed)));
        _busMock.Verify(bus => bus.Publish(It.Is<CreateUserPasswordFailedEvent>(createUserPasswordFailedEvent => createUserPasswordFailedEvent.RequestId == _requestId
        && createUserPasswordFailedEvent.UserId == _userId
        && createUserPasswordFailedEvent.Message == "Cannot create user password"
        ), null));
    }
}
