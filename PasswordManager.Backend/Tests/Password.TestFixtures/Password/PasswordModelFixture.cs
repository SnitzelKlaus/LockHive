using PasswordManager.Password.Domain.Password;

namespace PasswordManager.Password.TestFixtures.Password;
public static class PasswordModelFixture
{
    public static PasswordModelBuilder Builder() => new();

    public sealed class PasswordModelBuilder
    {
        private Guid _passwordId = Guid.NewGuid();
        private Guid _userId = Guid.NewGuid();
        private DateTime _createdUtc = DateTime.UtcNow.AddDays(-1);
        private DateTime _modifiedUtc = DateTime.UtcNow;
        private bool _deleted = false;
        private string _url = "url";
        private string _friendlyName = "friendlyName";
        private string _username = "username";
        private string _password = "password";

        internal PasswordModelBuilder() { }

        public PasswordModel Build()
        {
            var passwordModel = new PasswordModel(_passwordId, _createdUtc, _modifiedUtc, _deleted, _userId, _url, _friendlyName, _username, _password);

            return passwordModel;
        }

        public PasswordModelBuilder WithId(Guid passwordId)
        {
            _passwordId = passwordId;
            return this;
        }

        public PasswordModelBuilder IsDeleted()
        {
            _deleted = true;
            return this;
        }
    }
}
