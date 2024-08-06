using PasswordManager.Users.Domain.User;

namespace PasswordManager.Users.TestFixtures.Users;
public static class UserModelFixture
{
    public static UserModelBuilder Builder() => new();

    public sealed class UserModelBuilder
    {
        private Guid _userId = Guid.NewGuid();
        private string _firebaseId = Guid.NewGuid().ToString();
        private DateTime _createdUtc = DateTime.UtcNow.AddDays(-1);
        private DateTime _modifiedUtc = DateTime.UtcNow;
        private bool _deleted = false;
        private string _secretKey = "secretKey";

        internal UserModelBuilder() { }

        public UserModel Build()
        {
            var userModel = new UserModel(_userId, _createdUtc, _modifiedUtc, _deleted, _firebaseId, _secretKey);

            return userModel;
        }

        public UserModelBuilder WithId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public UserModelBuilder IsDeleted()
        {
            _deleted = true;
            return this;
        }
    }
}
