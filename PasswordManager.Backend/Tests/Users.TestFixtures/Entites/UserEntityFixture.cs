using PasswordManager.Users.Infrastructure.UserRepository;

namespace PasswordManager.Users.TestFixtures.Entities;
public class UserEntityFixture
{
    public static UserEntityBuilder Builder()
    {
        return new UserEntityBuilder();
    }

    public class UserEntityBuilder
    {
        private Guid _userId = Guid.NewGuid();
        private string _firebaseId = Guid.NewGuid().ToString();
        private string _secretKey = "secretKey";

        private DateTime _createdDate = DateTime.UtcNow.AddMonths(-1).AddDays(-1);
        private DateTime _modifiedDate = DateTime.UtcNow.AddMonths(-1).AddDays(-1);

        internal UserEntityBuilder()
        {
        }

        public UserEntity Build()
        {
            var userEntity = new UserEntity(_userId, _createdDate, _modifiedDate, _firebaseId, _secretKey);
            return userEntity;
        }

        public UserEntityBuilder Id(Guid id)
        {
            _userId = id;
            return this;
        }
    }
}
