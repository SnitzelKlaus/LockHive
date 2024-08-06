using PasswordManager.Users.Infrastructure.BaseRepository;
using System;

namespace PasswordManager.Users.Infrastructure.UserRepository;
/// <summary>
/// Represents a user entity within the system. This entity includes user-specific information such as Firebase ID and a secret key.
/// Inherits from <see cref="BaseEntity"/> to leverage common entity properties such as ID and timestamps for creation and modification.
/// </summary>
public class UserEntity : BaseEntity
{
    public string FirebaseId { get; }
    public string SecretKey { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserEntity"/> class with the specified details.
    /// </summary>
    /// <param name="id">The unique identifier for the user.</param>
    /// <param name="createdUtc">The UTC timestamp when the user entity was created.</param>
    /// <param name="modifiedUtc">The UTC timestamp when the user entity was last modified.</param>
    /// <param name="firebaseId">The Firebase ID associated with the user.</param>
    /// <param name="secretKey">The secret key associated with the user.</param>
    public UserEntity(Guid id, DateTime createdUtc, DateTime modifiedUtc, string firebaseId, string secretKey) : base(id, createdUtc, modifiedUtc)
    {
        FirebaseId = firebaseId;
        SecretKey = secretKey;
    }
}