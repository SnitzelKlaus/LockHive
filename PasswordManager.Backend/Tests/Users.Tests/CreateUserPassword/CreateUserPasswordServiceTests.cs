using Moq;
using NUnit.Framework;
using PasswordManager.Users.ApplicationServices.Components;
using PasswordManager.Users.ApplicationServices.Operations;
using PasswordManager.Users.ApplicationServices.Repositories.User;
using PasswordManager.Users.ApplicationServices.UserPassword.CreateUserPassword;
using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.Domain.User;
using PasswordManager.Users.TestFixtures.Operations;
using PasswordManager.Users.TestFixtures.Users;
using Rebus.Bus;
using Users.Messages.CreateUserPassword;

namespace PasswordManager.Users.Tests.CreateUserPassword;
internal sealed class CreateUserPasswordServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IOperationService> _operationServiceMock;
    private Mock<IBus> _busMock;
    private Mock<IPasswordComponent> _passwordComponentMock;
    private Mock<IKeyVaultComponent> _keyVaultComponentMock;

    private ICreateUserPasswordService _createUserPasswordService;
    private static Guid _userId = Guid.NewGuid();
    private static string _createdBy = Guid.NewGuid().ToString();
    private static string _requestId = Guid.NewGuid().ToString();

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _operationServiceMock = new Mock<IOperationService>();
        _busMock = new Mock<IBus>();
        _passwordComponentMock = new Mock<IPasswordComponent>();
        _keyVaultComponentMock = new Mock<IKeyVaultComponent>();

        _createUserPasswordService = new CreateUserPasswordService(_userRepositoryMock.Object,
                                                                   _operationServiceMock.Object,
                                                                   _busMock.Object,
                                                                   _passwordComponentMock.Object,
                                                                   _keyVaultComponentMock.Object);
    }

    #region RequestCreateUserPassword

    [Test]
    public async Task RequestCreateUserPassword_ReturnsOperationAcceptedResult()
    {
        var userModelFixture = UserModelFixture.Builder().WithId(_userId).Build();
        var userPasswordModelFixture = UserPasswordModelFixture.Builder().WithId(_userId).Build();
        var operationDetails = new OperationDetails(_createdBy);
        var operation = OperationFixture.Builder().WithName(OperationName.CreateUserPassword).Build();

        _userRepositoryMock.Setup(user => user.Get(It.IsAny<Guid>())).ReturnsAsync(userModelFixture);
        _keyVaultComponentMock.Setup(password => password.CreateEncryptedPassword(userPasswordModelFixture, userModelFixture.SecretKey)).ReturnsAsync("EncryptedPassword");
        _operationServiceMock.Setup(operation => operation.QueueOperation(It.IsAny<Operation>())).ReturnsAsync(operation);
        var requestCreateUserPasswordResult = await _createUserPasswordService.RequestCreateUserPassword(userPasswordModelFixture, operationDetails);

        Assert.That(requestCreateUserPasswordResult, Is.Not.Null);
        Assert.That(requestCreateUserPasswordResult.Status, Is.EqualTo(OperationResultStatus.Accepted));
    }

    [Test]
    public async Task RequestCreateUserPassword_ReturnsOperationAcceptedResult_WhenCreateUserPasswordOperationsHasBeenQueued()
    {
        var userModelFixture = UserModelFixture.Builder().WithId(_userId).Build();
        var userPasswordModelFixture = UserPasswordModelFixture.Builder().WithId(_userId).Build();
        var operationDetails = new OperationDetails(_createdBy);
        var operation = OperationFixture.Builder().WithName(OperationName.CreateUserPassword).Build();

        _userRepositoryMock.Setup(user => user.Get(It.IsAny<Guid>())).ReturnsAsync(userModelFixture);
        _keyVaultComponentMock.Setup(password => password.CreateEncryptedPassword(userPasswordModelFixture, userModelFixture.SecretKey)).ReturnsAsync("EncryptedPassword");
        _operationServiceMock.Setup(operation => operation.QueueOperation(It.IsAny<Operation>())).ReturnsAsync(operation);

        var requestCreateUserPasswordResult = await _createUserPasswordService.RequestCreateUserPassword(userPasswordModelFixture, operationDetails);

        Assert.That(requestCreateUserPasswordResult, Is.Not.Null);
        Assert.That(requestCreateUserPasswordResult.Status, Is.EqualTo(OperationResultStatus.Accepted));

        _operationServiceMock.Verify(operation => operation.QueueOperation(It.Is<Operation>(ops => ValidateOperationData(ops, userPasswordModelFixture))));
    }

    [Test]
    public async Task RequestCreateUserPassword_ReturnsOperationAcceptedResult_WhenCreateUserPasswordCommandHasBeenSend()
    {
        var userModelFixture = UserModelFixture.Builder().WithId(_userId).Build();
        var userPasswordModelFixture = UserPasswordModelFixture.Builder().WithId(_userId).Build();
        var operationDetails = new OperationDetails(_createdBy);
        var operation = OperationFixture.Builder().WithRequestId(_requestId).WithName(OperationName.CreateUserPassword).Build();

        _userRepositoryMock.Setup(user => user.Get(It.IsAny<Guid>())).ReturnsAsync(userModelFixture);
        _keyVaultComponentMock.Setup(password => password.CreateEncryptedPassword(userPasswordModelFixture, userModelFixture.SecretKey)).ReturnsAsync("EncryptedPassword");
        _operationServiceMock.Setup(operation => operation.QueueOperation(It.IsAny<Operation>())).ReturnsAsync(operation);

        var requestCreateUserPasswordResult = await _createUserPasswordService.RequestCreateUserPassword(userPasswordModelFixture, operationDetails);

        Assert.That(requestCreateUserPasswordResult, Is.Not.Null);
        Assert.That(requestCreateUserPasswordResult.Status, Is.EqualTo(OperationResultStatus.Accepted));

        _busMock.Verify(bus => bus.Send(It.Is<CreateUserPasswordCommand>(cmd => cmd.RequestId == _requestId), null));
    }

    [Test]
    public async Task RequestCreateUserPassword_ReturnsOperationInvalidStateResult_WhenUserIsNotFound()
    {
        var userPasswordModelFixture = UserPasswordModelFixture.Builder().WithId(_userId).Build();
        var operationDetails = new OperationDetails(_createdBy);

        _userRepositoryMock.Setup(user => user.Get(It.IsAny<Guid>())).ReturnsAsync(null as UserModel);
        
        var requestCreateUserPasswordResult = await _createUserPasswordService.RequestCreateUserPassword(userPasswordModelFixture, operationDetails);

        Assert.That(requestCreateUserPasswordResult, Is.Not.Null);
        Assert.That(requestCreateUserPasswordResult.Status, Is.EqualTo(OperationResultStatus.InvalidOperationRequest));

        _operationServiceMock.Verify(operation => operation.QueueOperation(It.IsAny<Operation>()), Times.Never);
        _keyVaultComponentMock.Verify(password => password.CreateEncryptedPassword(It.IsAny<UserPasswordModel>(), It.IsAny<string>()), Times.Never);
        _busMock.Verify(bus => bus.Send(It.IsAny<CreateUserPasswordCommand>(), null), Times.Never);
    }

    [Test]
    public async Task RequestCreateUserPassword_ReturnsOperationInvalidStateResult_WhenUserIsMarkedAsDeleted()
    {
        var userModelFixture = UserModelFixture.Builder().WithId(_userId).IsDeleted().Build();
        var userPasswordModelFixture = UserPasswordModelFixture.Builder().WithId(_userId).Build();
        var operationDetails = new OperationDetails(_createdBy);

        _userRepositoryMock.Setup(user => user.Get(It.IsAny<Guid>())).ReturnsAsync(userModelFixture);

        var requestCreateUserPasswordResult = await _createUserPasswordService.RequestCreateUserPassword(userPasswordModelFixture, operationDetails);

        Assert.That(requestCreateUserPasswordResult, Is.Not.Null);
        Assert.That(requestCreateUserPasswordResult.Status, Is.EqualTo(OperationResultStatus.InvalidOperationRequest));

        _operationServiceMock.Verify(operation => operation.QueueOperation(It.IsAny<Operation>()), Times.Never);
        _keyVaultComponentMock.Verify(password => password.CreateEncryptedPassword(It.IsAny<UserPasswordModel>(), It.IsAny<string>()), Times.Never);
        _busMock.Verify(bus => bus.Send(It.IsAny<CreateUserPasswordCommand>(), null), Times.Never);
    }

    [TestCase(null)]
    [TestCase("")]
    public async Task RequestCreateUserPassword_ReturnsOperationInvalidStateResult_WhenEncryptedPasswordIsNullOrEmptyString(string invalidEncryptedPassword)
    {
        var userModelFixture = UserModelFixture.Builder().WithId(_userId).Build();
        var userPasswordModelFixture = UserPasswordModelFixture.Builder().WithId(_userId).Build();
        var operationDetails = new OperationDetails(_createdBy);

        _userRepositoryMock.Setup(user => user.Get(It.IsAny<Guid>())).ReturnsAsync(userModelFixture);
        _keyVaultComponentMock.Setup(password => password.CreateEncryptedPassword(It.IsAny<UserPasswordModel>(), It.IsAny<string>())).ReturnsAsync(invalidEncryptedPassword);

        var requestCreateUserPasswordResult = await _createUserPasswordService.RequestCreateUserPassword(userPasswordModelFixture, operationDetails);

        Assert.That(requestCreateUserPasswordResult, Is.Not.Null);
        Assert.That(requestCreateUserPasswordResult.Status, Is.EqualTo(OperationResultStatus.InvalidOperationRequest));

        _keyVaultComponentMock.Verify(password => password.CreateEncryptedPassword(It.IsAny<UserPasswordModel>(), It.IsAny<string>()));
        _operationServiceMock.Verify(operation => operation.QueueOperation(It.IsAny<Operation>()), Times.Never);
        _busMock.Verify(bus => bus.Send(It.IsAny<CreateUserPasswordCommand>(), null), Times.Never);
    }

    [Test]
    public async Task RequestCreateUserPassword_ReturnsOperationInvalidStateResult_WhenKeyVaultComponentExceptionIsCaught()
    {
        var userModelFixture = UserModelFixture.Builder().WithId(_userId).Build();
        var userPasswordModelFixture = UserPasswordModelFixture.Builder().WithId(_userId).Build();
        var operationDetails = new OperationDetails(_createdBy);

        _userRepositoryMock.Setup(user => user.Get(It.IsAny<Guid>())).ReturnsAsync(userModelFixture);
        _keyVaultComponentMock.Setup(password => password.CreateEncryptedPassword(It.IsAny<UserPasswordModel>(), It.IsAny<string>()))
            .ThrowsAsync(new KeyVaultComponentException("Cannot encrypt password", new Exception()));

        var requestCreateUserPasswordResult = await _createUserPasswordService.RequestCreateUserPassword(userPasswordModelFixture, operationDetails);

        Assert.That(requestCreateUserPasswordResult, Is.Not.Null);
        Assert.That(requestCreateUserPasswordResult.Status, Is.EqualTo(OperationResultStatus.InvalidOperationRequest));

        _keyVaultComponentMock.Verify(password => password.CreateEncryptedPassword(It.IsAny<UserPasswordModel>(), It.IsAny<string>()));
        _operationServiceMock.Verify(operation => operation.QueueOperation(It.IsAny<Operation>()), Times.Never);
        _busMock.Verify(bus => bus.Send(It.IsAny<CreateUserPasswordCommand>(), null), Times.Never);
    }

    private bool ValidateOperationData(Operation ops, UserPasswordModel userPasswordModel)
    {
        return ops.UserId == _userId
        && ops.CreatedBy == _createdBy
        && ops.Name == OperationName.CreateUserPassword
        && ops.Data.ContainsKey("createUserPasswordUrl") && ops.Data["createUserPasswordUrl"] == userPasswordModel.Url
        && ops.Data.ContainsKey("createUserPasswordFriendlyName") && ops.Data["createUserPasswordFriendlyName"] == userPasswordModel.FriendlyName
        && ops.Data.ContainsKey("createUserPasswordUsername") && ops.Data["createUserPasswordUsername"] == userPasswordModel.Username
        && ops.Data.ContainsKey("createUserPasswordPassword") && ops.Data["createUserPasswordPassword"] == userPasswordModel.Password;
    }
    #endregion

    #region CreateUserPassword

    [Test]
    public async Task CreateUserPassword_CallsPasswordComponent()
    {
        var userPasswordModel = UserPasswordModelFixture.Builder().Build();

        await _createUserPasswordService.CreateUserPassword(userPasswordModel);

        _passwordComponentMock.Verify(component => component.CreateUserPassword(It.Is<UserPasswordModel>(user => user.UserId == userPasswordModel.UserId 
        && user.PasswordId == userPasswordModel.PasswordId
        && user.Url == userPasswordModel.Url
        && user.FriendlyName == userPasswordModel.FriendlyName
        && user.Username == userPasswordModel.Username
        && user.Password == userPasswordModel.Password
        )));
    }

    [Test]
    public void CreateUserPassword_ThrowsException_WhenPasswordComponentExceptionIsCaught()
    {
        var userPasswordModel = UserPasswordModelFixture.Builder().Build();

        _passwordComponentMock.Setup(component => component.CreateUserPassword(It.IsAny<UserPasswordModel>()))
            .ThrowsAsync(new PasswordComponentException("Cannot create user password", new Exception()));

        Assert.ThrowsAsync<CreateUserPasswordServiceException>(() => _createUserPasswordService.CreateUserPassword(userPasswordModel));
    }
    #endregion
}
