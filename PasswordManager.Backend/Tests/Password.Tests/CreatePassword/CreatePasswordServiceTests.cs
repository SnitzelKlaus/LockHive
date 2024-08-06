using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Password.Messages.CreatePassword;
using PasswordManager.Password.ApplicationServices.Operations;
using PasswordManager.Password.ApplicationServices.Password.CreatePassword;
using PasswordManager.Password.ApplicationServices.Repositories.Password;
using PasswordManager.Password.Domain.Operations;
using PasswordManager.Password.Domain.Password;
using PasswordManager.Password.TestFixtures.Operations;
using PasswordManager.Password.TestFixtures.Password;
using Rebus.Bus;

namespace PasswordManager.Password.Tests.CreatePassword;
internal class CreatePasswordServiceTests
{
    private Mock<IOperationService> _operationServiceMock;
    private Mock<IBus> _busMock;
    private Mock<ILogger<CreatePasswordService>> _logger;
    private Mock<IPasswordRepository> _passwordRepositoryMock;

    private ICreatePasswordService _createPasswordService;

    private static Guid _passwordId = Guid.NewGuid();
    private readonly string _createdBy = Guid.NewGuid().ToString();
    private readonly string _requestId = Guid.NewGuid().ToString();

    [SetUp]
    public void Setup()
    {
        _operationServiceMock = new Mock<IOperationService>();
        _busMock = new Mock<IBus>();
        _logger = new Mock<ILogger<CreatePasswordService>>();
        _passwordRepositoryMock = new Mock<IPasswordRepository>();

        _createPasswordService = new CreatePasswordService(_operationServiceMock.Object, _busMock.Object, _logger.Object, _passwordRepositoryMock.Object);
    }

    #region RequestCreatePassword

    [Test]
    public async Task RequestCreatePassword_ReturnOperationAcceptedResult_WhenOperationHasBeenQueuedAndCommandHasBeenSent()
    {
        var passwordModelFixture = PasswordModelFixture.Builder().WithId(_passwordId).Build();
        var operationDetails = new OperationDetails(_createdBy);
        var operationFixture = OperationFixture.Builder().WithRequestId(_requestId).WithPasswordId(_passwordId).WithName(OperationName.CreatePassword).Build();

        _operationServiceMock.Setup(operation => operation.QueueOperation(It.IsAny<Operation>())).ReturnsAsync(operationFixture);

        var requestCreatePasswordResult = await _createPasswordService.RequestCreatePassword(passwordModelFixture, operationDetails);

        Assert.That(requestCreatePasswordResult, Is.Not.Null);
        Assert.That(requestCreatePasswordResult.Status, Is.EqualTo(OperationResultStatus.Accepted));

        _operationServiceMock.Verify(operation => operation.QueueOperation(It.Is<Operation>(ops => ValidateOperationData(ops, passwordModelFixture))));
        _busMock.Verify(bus => bus.Send(It.Is<CreatePasswordCommand>(cmd => cmd.RequestId == _requestId), null));
    }

    private bool ValidateOperationData(Operation ops, PasswordModel passwordModel)
    {
        return ops.PasswordId == _passwordId
        && ops.CreatedBy == _createdBy
        && ops.Name == OperationName.CreatePassword
        && ops.Data.ContainsKey(OperationDataConstants.PasswordCreateUserId) && ops.Data["createPasswordUserId"] == passwordModel.UserId.ToString()
        && ops.Data.ContainsKey(OperationDataConstants.PasswordCreateUrl) && ops.Data["createPasswordUrl"] == passwordModel.Url
        && ops.Data.ContainsKey(OperationDataConstants.PasswordCreateFriendlyName) && ops.Data["createPasswordFriendlyName"] == passwordModel.FriendlyName
        && ops.Data.ContainsKey(OperationDataConstants.PasswordCreateUsername) && ops.Data["createPasswordUsername"] == passwordModel.Username
        && ops.Data.ContainsKey(OperationDataConstants.PasswordCreatePassword) && ops.Data["createPasswordPassword"] == passwordModel.Password;
    }
    #endregion

    #region CreatePassword

    [Test]
    public async Task CreatePassword_CallRepositoryToAddPassword()
    {
        var passwordModelFixture = PasswordModelFixture.Builder().Build();

        await _createPasswordService.CreatePassword(passwordModelFixture);

        _passwordRepositoryMock.Setup(repo => repo.Add(It.Is<PasswordModel>(passwordModel => passwordModel.Id == passwordModelFixture.Id
        && passwordModel.UserId == passwordModelFixture.UserId
        && passwordModel.Url == passwordModelFixture.Url
        && passwordModel.FriendlyName == passwordModelFixture.FriendlyName
        && passwordModel.Username == passwordModelFixture.Username
        && passwordModel.Password == passwordModelFixture.Username
        )));
    }

    [Test]
    public async Task CreatePassword_ThrowsCreatePasswordServiceException_WhenExceptionIsCaught()
    {
        var passwordModelFixture = PasswordModelFixture.Builder().Build();

        _passwordRepositoryMock.Setup(repo => repo.Add(It.IsAny<PasswordModel>())).ThrowsAsync(new Exception());

        Assert.ThrowsAsync<CreatePasswordServiceException>(() => _createPasswordService.CreatePassword(passwordModelFixture));
    }
    #endregion
}
