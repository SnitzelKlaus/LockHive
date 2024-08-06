using PasswordManager.Users.Domain.Operations;

namespace PasswordManager.Users.TestFixtures.Operations;
public static class OperationFixture
{
    public static OperationBuilder Builder()
    {
        return new OperationBuilder();
    }

    public sealed class OperationBuilder
    {
        private Guid _id = Guid.NewGuid();

        private DateTime _createdUtc = DateTime.UtcNow.AddMonths(-1).AddDays(-1);
        private DateTime _modifiedUtc = DateTime.UtcNow.AddMonths(-1).AddHours(-1);
        private DateTime? _completedUtc;

        private OperationName _operationName = OperationName.CreateUserPassword;
        private OperationStatus _operationStatus = OperationStatus.Queued;
        private Guid _userId = Guid.NewGuid();

        private string _requestId = "request-id";
        private string _createdBy = "created by";
        private Dictionary<string, string>? _data;

        internal OperationBuilder()
        {
        }

        public Operation Build()
        {
            var operationEntity = new Operation(_id, _requestId, _createdBy, _userId, _operationName, _operationStatus, _createdUtc, _modifiedUtc, _completedUtc, _data);

            return operationEntity;
        }

        public OperationBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public OperationBuilder WithUserId(Guid passwordId)
        {
            _userId = passwordId;
            return this;
        }

        public OperationBuilder WithCreatedBy(string createdBy)
        {
            _createdBy = createdBy;
            return this;
        }

        public OperationBuilder WithRequestId(string requestId)
        {
            _requestId = requestId;
            return this;
        }

        public OperationBuilder WithName(OperationName name)
        {
            _operationName = name;
            return this;
        }

        public OperationBuilder WithStatus(OperationStatus status)
        {
            _operationStatus = status;
            return this;
        }

        public OperationBuilder WithCreatedUtc(DateTime createdUtc)
        {
            _createdUtc = createdUtc;
            return this;
        }

        public OperationBuilder WithModifiedUtc(DateTime modifiedUtc)
        {
            _modifiedUtc = modifiedUtc;
            return this;
        }

        public OperationBuilder WithCompletedUtc(DateTime? completedUtc)
        {
            _completedUtc = completedUtc;
            return this;
        }

        public OperationBuilder WithClearData()
        {
            _data = null;
            return this;
        }

        public OperationBuilder WithAddData(string key, string value)
        {
            if (_data == null)
                _data = new Dictionary<string, string>();
            _data.Add(key, value);
            return this;
        }
    }
}