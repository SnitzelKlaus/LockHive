namespace PasswordManager.Password.Domain.Operations
{
    public sealed class OperationDataConstants
    {
        #region Create Password
        public static string PasswordCreateUserId => "createPasswordUserId";
        public static string PasswordCreateUrl => "createPasswordUrl";
        public static string PasswordCreateFriendlyName => "createPasswordFriendlyName";
        public static string PasswordCreateUsername => "createPasswordUsername";
        public static string PasswordCreatePassword => "createPasswordPassword";
        #endregion

        #region Update Password
        public static string CurrentPasswordUrl => "currentPasswordUrl";
        public static string CurrentPasswordFriendlyName => "currentPasswordFriendlyName";
        public static string CurrentPasswordUsername => "currentPasswordUsername";
        public static string CurrentPasswordPassword => "currentPasswordPassword";

        public static string NewPasswordUrl => "newPasswordUrl";
        public static string NewPasswordFriendlyName => "newPasswordFriendlyName";
        public static string NewPasswordUsername => "newPasswordUsername";
        public static string NewPasswordPassword => "newPasswordPassword";
        #endregion
    }
}
