using PasswordManager.Users.Infrastructure.UserRepository;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PasswordManager.Users.Api.Service.CurrentUser
{
    /// <summary>
    /// Represents the current user session, providing functionalities to retrieve and manage the current user entity.
    /// </summary>
    public class CurrentUser : ICurrentUser
    {
        private readonly ClaimsPrincipal _claimsPrincipal;
        private readonly UserContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentUser"/> class.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal for the current user's identity.</param>
        /// <param name="context">The user context for database access.</param>
        public CurrentUser(ClaimsPrincipal claimsPrincipal, UserContext context)
        {
            _claimsPrincipal = claimsPrincipal;
            _context = context;
        }
        private UserEntity currentUser { get; set; }

        /// <summary>
        /// Retrieves the <see cref="UserEntity"/> representing the current user, creating a new user if necessary.
        /// </summary>
        /// <returns>The current <see cref="UserEntity"/>.</returns>
        public UserEntity GetUser()
        {
            if (currentUser == null)
            {
                var user = this._context.Users!.FirstOrDefault(u => u.FirebaseId == _claimsPrincipal.FindFirstValue("id"));

                if (user == null)
                {
                    user = new UserEntity(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow,
                        _claimsPrincipal.FindFirstValue("id"), GenerateSecretKey(128));
                    _context.Users!.Add(user);

                    _context.SaveChanges();
                }

                currentUser = user;
            }

            return currentUser;

        }

        /// <summary>
        /// Generates a secret key of the specified bit length.
        /// </summary>
        /// <param name="bitLength">The bit length of the key to generate. Must be 128, 192, or 256.</param>
        /// <returns>A base64 string representation of the generated secret key.</returns>
        /// <exception cref="ArgumentException">Thrown when an unsupported bit length is specified.</exception>
        public string GenerateSecretKey(int bitLength)
        {
            if (bitLength != 128 && bitLength != 192 && bitLength != 256)
            {
                throw new ArgumentException("AES key length must be 128, 192, or 256 bits.");
            }

            int byteLength = bitLength / 8;
            byte[] keyBytes = GenerateRandomBytes(byteLength);
            return Convert.ToBase64String(keyBytes);
        }

        /// <summary>
        /// Generates a byte array of random bytes of the specified length.
        /// </summary>
        /// <param name="length">The length of the byte array to generate.</param>
        /// <returns>A byte array containing random bytes.</returns>
        private byte[] GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];
            RandomNumberGenerator.Create().GetBytes(randomBytes);
            return randomBytes;
        }

    }
}
