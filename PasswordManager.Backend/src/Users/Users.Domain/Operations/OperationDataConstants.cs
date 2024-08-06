namespace PasswordManager.Users.Domain.Operations
{
    /// <summary>
    /// Contains constants for operation data related to user passwords.
    /// </summary>
    public sealed class OperationDataConstants
    {
        #region Create Password
        public static string CreateUserPasswordUrl => "createUserPasswordUrl";
        public static string CreateUserPasswordFriendlyName => "createUserPasswordFriendlyName";
        public static string CreateUserPasswordUsername => "createUserPasswordUsername";
        public static string CreateUserPasswordPassword => "createUserPasswordPassword";
        #endregion

        #region Update Password
        public static string UserPasswordId => "userPasswordId";
        public static string NewUserPasswordUrl => "newPasswordUrl";
        public static string NewUserPasswordFriendlyName => "newPasswordFriendlyName";
        public static string NewUserPasswordUsername => "newPasswordUsername";
        public static string NewUserPasswordPassword => "newPasswordPassword";
        #endregion

    }
}
