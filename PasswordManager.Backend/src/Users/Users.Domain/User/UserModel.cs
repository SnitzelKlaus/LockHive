namespace PasswordManager.Users.Domain.User;
/// <summary>
/// Represents a user model.
/// </summary>
public class UserModel : BaseModel
{
    public string FirebaseId { get; }
    public string SecretKey { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserModel"/> class.
    /// </summary>
    /// <param name="id">The user's ID.</param>
    /// <param name="createdUtc">The creation date of the user.</param>
    /// <param name="modifiedUtc">The modification date of the user.</param>
    /// <param name="deleted">A value indicating whether the user is deleted.</param>
    /// <param name="firebaseId">The Firebase ID associated with the user.</param>
    /// <param name="secretKey">The secret key associated with the user.</param>
    public UserModel(Guid id, DateTime createdUtc, DateTime modifiedUtc, bool deleted, string firebaseId, string secretKey) : base(id, createdUtc, modifiedUtc, deleted)
    {
        FirebaseId = firebaseId;
        SecretKey = secretKey;
    }
}
