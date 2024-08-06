using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PasswordManager.Users.Infrastructure.UserRepository;

namespace PasswordManager.Users.Api.Service.Handlers
{
    /// <summary>
    /// Handles authentication of users through Firebase, extending the default authentication handler.
    /// </summary>
    public class FirebaseUserAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        // The prefix used for Bearer tokens in the Authorization header.
        private const string BEARER_PREFIX = "Bearer ";

        private readonly FirebaseApp _firebaseApp;
        private readonly ILogger<FirebaseUserAuthenticationHandler> _logger;
        protected readonly UserContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirebaseUserAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">Monitor for the options instance.</param>
        /// <param name="logger">Factory to create a logger from.</param>
        /// <param name="encoder">Encoder for the URLs.</param>
        /// <param name="clock">System clock.</param>
        /// <param name="firebaseApp">Firebase application instance.</param>
        /// <param name="context">User context for accessing user data.</param>
        public FirebaseUserAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            FirebaseApp firebaseApp,
            UserContext context)
            : base(options, logger, encoder, clock)
        {
            _firebaseApp = firebaseApp;
            this._logger = logger.CreateLogger<FirebaseUserAuthenticationHandler>();
            _context = context;
        }

        /// <summary>
        /// Handles the authentication asynchronously by validating the provided bearer token.
        /// </summary>
        /// <returns>The authentication result after validating the token.</returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            this._logger.LogInformation("Calling HandleAuthenticateAsync for FirebaseUserAuthenticationHandler");
            if (!Context.Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("No authorization");
            }

            string? bearerToken = Context.Request.Headers["Authorization"];

            //Remove this when development is done
            if (bearerToken == "test")
            {
                var record = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync("dvuust@gmail.com");
                var claims = new List<Claim>
                {
                    new Claim(FirebaseUserClaimType.ID, record.Uid ?? ""),
                    new Claim(FirebaseUserClaimType.EMAIL, record.Email ?? ""),
                    new Claim(FirebaseUserClaimType.EMAIL_VERIFIED, record.EmailVerified.ToString() ?? ""),
                    new Claim(FirebaseUserClaimType.USERNAME, record.DisplayName ?? ""),
                }.AsEnumerable();
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(claims, nameof(ClaimsIdentity))
                });


                return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, JwtBearerDefaults.AuthenticationScheme));
            }

            if (bearerToken == null || !bearerToken.StartsWith(BEARER_PREFIX))
            {
                return AuthenticateResult.Fail("Invalid scheme.");
            }

            string token = bearerToken.Substring(BEARER_PREFIX.Length);

            try
            {
                FirebaseToken firebaseToken = await FirebaseAuth.GetAuth(_firebaseApp).VerifyIdTokenAsync(token);

                return AuthenticateResult.Success(CreateAuthenticationTicket(firebaseToken));
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }

        /// <summary>
        /// Creates an authentication ticket from a validated Firebase token.
        /// </summary>
        /// <param name="firebaseToken">The validated Firebase token.</param>
        /// <returns>An authentication ticket.</returns>
        private AuthenticationTicket CreateAuthenticationTicket(FirebaseToken firebaseToken)
        {
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(ToClaims(firebaseToken.Claims), nameof(ClaimsIdentity))
            });

            return new AuthenticationTicket(claimsPrincipal, JwtBearerDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Converts the claims from a Firebase token into a list of <see cref="Claim"/>.
        /// </summary>
        /// <param name="claims">The claims from the Firebase token.</param>
        /// <returns>A list of claims for the authenticated user.</returns>
        private IEnumerable<Claim> ToClaims(IReadOnlyDictionary<string, object> claims)
        {
            return new List<Claim>
            {
                new Claim(FirebaseUserClaimType.ID, claims.GetValueOrDefault("user_id", "").ToString()!),
                new Claim(FirebaseUserClaimType.EMAIL, claims.GetValueOrDefault("email", "").ToString()!),
                new Claim(FirebaseUserClaimType.EMAIL_VERIFIED, claims.GetValueOrDefault("email_verified", "").ToString()!),
                new Claim(FirebaseUserClaimType.USERNAME, claims.GetValueOrDefault("name", "").ToString()!),
            };
        }
    }
}