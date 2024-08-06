using Microsoft.Extensions.Logging;
using Moq;
using Password.Worker.Service.CreatePassword;
using Rebus.Bus;
using PasswordManager.Password.ApplicationServices.Operations;
using NUnit.Framework;
using Password.Messages.CreatePassword;
using PasswordManager.Password.TestFixtures.Operations;
using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.TestFixtures.Password;
using PasswordManager.Password.Domain.Password;
using PasswordManager.Password.ApplicationServices.Password.CreatePassword;

namespace PasswordManager.Password.Tests.CreatePassword;
internal class CreatePasswordCommandHandlerTests
{
    private Mock<ICreatePasswordService> _createPasswordServiceMock;
    private Mock<IOperationService> _operationServiceMock;
    private Mock<ILogger<CreatePasswordCommandHandler>> _loggerMock;
    private Mock<IBus> _busMock;

    private CreatePasswordCommandHandler _createPasswordCommandHandler;
    private readonly string _requestId = Guid.NewGuid().ToString();
    private static Guid _passwordId = Guid.NewGuid();

    [SetUp]
    public void Setup()
    {
        _createPasswordServiceMock = new Mock<ICreatePasswordService>();
        _operationServiceMock = new Mock<IOperationService>();
        _loggerMock = new Mock<ILogger<CreatePasswordCommandHandler>>();
        _busMock = new Mock<IBus>();

        _createPasswordCommandHandler = new CreatePasswordCommandHandler(_createPasswordServiceMock.Object, _operationServiceMock.Object, _loggerMock.Object, _busMock.Object);
    }

    [Test]
    public async Task Handle_PublishSuccessEventAndMarkOperationAsCompleted()
    {
        var completedOperationStatus = OperationStatus.Completed;
        var createPasswordCommand = new CreatePasswordCommand(_requestId);
        var passwordModelFixture = PasswordModelFixture.Builder().WithId(_passwordId).Build();
        var queuedOperationFixture = CreatePasswordOperationAsQueued(passwordModelFixture);

        _operationServiceMock.Setup(operation => operation.GetOperationByRequestId(It.IsAny<string>())).ReturnsAsync(queuedOperationFixture);

        await _createPasswordCommandHandler.Handle(createPasswordCommand);

        _operationServiceMock.Verify(operation => operation.GetOperationByRequestId(It.Is<string>(requestId => requestId == _requestId)));
        _operationServiceMock.Verify(operation => operation.UpdateOperationStatus(It.Is<string>(requestId => requestId == _requestId), 
            It.Is<OperationStatus>(status => status == OperationStatus.Processing)));

        _createPasswordServiceMock.Verify(service => service.CreatePassword(It.Is<PasswordModel>(passwordModel => passwordModel.Id == _passwordId)));
        _operationServiceMock.Verify(operation => operation.UpdateOperationStatus(It.Is<string>(requestId => requestId == _requestId), 
            It.Is<OperationStatus>(status => status == completedOperationStatus)));
        _busMock.Verify(bus => bus.Publish(It.Is<CreatePasswordEvent>(createPasswordEvent => createPasswordEvent.RequestId ==  _requestId
            && createPasswordEvent.PasswordId == _passwordId), null));
    }

    [Test]
    public void Handle_ThrowsInvalidOperationException_WhenOperationIsNotFound()
    {
        var createPasswordCommand = new CreatePasswordCommand(_requestId);

        _operationServiceMock.Setup(operation => operation.GetOperationByRequestId(It.IsAny<string>())).ReturnsAsync(null as Operation);

        Assert.ThrowsAsync<InvalidOperationException>(() => _createPasswordCommandHandler.Handle(createPasswordCommand));

        _operationServiceMock.Verify(operation => operation.GetOperationByRequestId(It.Is<string>(requestId => requestId == _requestId)));
    }

    [Test]
    public async Task Handle_Handle_PublishFailedEventAndMarkOperationAsFailed_WhenCreatePasswordServiceExceptionIsCaught()
    {
        var failedOperationStatus = OperationStatus.Failed;
        var exceptionMessage = "Cannot create password";
        var createPasswordCommand = new CreatePasswordCommand(_requestId);
        var passwordModelFixture = PasswordModelFixture.Builder().WithId(_passwordId).Build();
        var queuedOperationFixture = CreatePasswordOperationAsQueued(passwordModelFixture);

        _operationServiceMock.Setup(operation => operation.GetOperationByRequestId(It.IsAny<string>())).ReturnsAsync(queuedOperationFixture);

        _createPasswordServiceMock.Setup(service => service.CreatePassword(It.IsAny<PasswordModel>())).ThrowsAsync(new CreatePasswordServiceException(exceptionMessage, new Exception()));

        await _createPasswordCommandHandler.Handle(createPasswordCommand);

        _operationServiceMock.Verify(operation => operation.UpdateOperationStatus(It.Is<string>(requestId => requestId == _requestId), It.Is<OperationStatus>(status => status == failedOperationStatus)));
        _busMock.Verify(bus => bus.Publish(It.Is<CreatePasswordFailedEvent>(createPasswordFailedEvent => createPasswordFailedEvent.RequestId == _requestId
        && createPasswordFailedEvent.PasswordId == _passwordId
        && createPasswordFailedEvent.Message == exceptionMessage
        ), null));
    }

    private Operation CreatePasswordOperationAsQueued(PasswordModel passwordModelFixture)
    {
        var operationFixture = OperationFixture.Builder().WithRequestId(_requestId).WithPasswordId(_passwordId).WithName(OperationName.CreatePassword).WithStatus(OperationStatus.Queued)
            .WithAddData(OperationDataConstants.PasswordCreateUserId, passwordModelFixture.UserId.ToString())
            .WithAddData(OperationDataConstants.PasswordCreateUrl, passwordModelFixture.Url)
            .WithAddData(OperationDataConstants.PasswordCreateFriendlyName, passwordModelFixture.FriendlyName)
            .WithAddData(OperationDataConstants.PasswordCreateUsername, passwordModelFixture.Username)
            .WithAddData(OperationDataConstants.PasswordCreatePassword, passwordModelFixture.Password)
            .Build();

        return operationFixture;
    }
}
