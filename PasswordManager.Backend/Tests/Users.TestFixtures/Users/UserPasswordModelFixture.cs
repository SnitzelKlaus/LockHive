using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.TestFixtures.Users;
public static class UserPasswordModelFixture
{
    public static UserPasswordModelBuilder Builder() => new();

    public sealed class UserPasswordModelBuilder
    {
        private Guid _userId = Guid.NewGuid();
        private Guid _passwordId = Guid.NewGuid();
        private string _url = "url";
        private string _friendlyName = "friendlyName";
        private string _username = "username";
        private string _password = "password";

        internal UserPasswordModelBuilder() { }

        public UserPasswordModel Build()
        {
            var passwordModel = new UserPasswordModel(_userId, _passwordId, _url, _friendlyName, _username, _password);

            return passwordModel;
        }

        public UserPasswordModelBuilder WithId(Guid userId)
        {
            _userId = userId;
            return this;
        }
    }
}
