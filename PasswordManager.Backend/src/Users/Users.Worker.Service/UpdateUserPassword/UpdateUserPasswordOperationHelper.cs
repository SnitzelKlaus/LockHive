using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.Domain.User;

namespace Users.Worker.Service.UpdateUserPassword
{
    /// <summary>
    /// Provides helper methods for updating user passwords in the system.
    /// </summary>
    public class UpdateUserPasswordOperationHelper
    {
        /// <summary>
        /// Maps the given operation to a <see cref="UserPasswordModel"/> for updating the user password.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="operation">The operation containing the user password update data.</param>
        /// <returns>A <see cref="UserPasswordModel"/> with the updated user password information.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required operation data is missing.</exception>
        internal static UserPasswordModel Map(Guid userId, Operation operation)
        {
            return UserPasswordModel.UpdateUserPassword(userId, GetPasswordId(operation), GetPasswordUrl(operation), GetPasswordLabel(operation), GetPasswordUsername(operation), GetPasswordKey(operation));
        }

        /// <summary>
        /// Retrieves specific user password operation data based on the provided constant.
        /// </summary>
        /// <param name="operation">The operation containing the user password data.</param>
        /// <param name="operationDataConstant">The constant key to retrieve the specific data.</param>
        /// <returns>The operation data associated with the specified constant.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the specified data is not found within the operation.</exception>
        private static string GetUserPasswordOperationData(Operation operation, string operationDataConstant)
        {
            if (operation.Data is null || operation.Data.TryGetValue(operationDataConstant, out var getPasswordOperationData) is false)
                throw new InvalidOperationException($"Could not find user password {operationDataConstant} in operation with request id {operation.RequestId} when creating user password");

            return getPasswordOperationData;
        }

        /// <summary>
        /// Gets the password ID from the operation data.
        /// </summary>
        /// <param name="operation">The operation to extract the password ID from.</param>
        /// <returns>The GUID representing the password ID.</returns>
        private static Guid GetPasswordId(Operation operation)
        {
            var getPasswordId = GetUserPasswordOperationData(operation, OperationDataConstants.UserPasswordId);
            return Guid.Parse(getPasswordId);
        }

        /// <summary>
        /// Gets the password URL from the operation data.
        /// </summary>
        /// <param name="operation">The operation to extract the password URL from.</param>
        /// <returns>The URL for the password.</returns>
        private static string GetPasswordUrl(Operation operation)
        {
            return GetUserPasswordOperationData(operation, OperationDataConstants.NewUserPasswordUrl);
        }

        /// <summary>
        /// Gets the password label from the operation data.
        /// </summary>
        /// <param name="operation">The operation to extract the password label from.</param>
        /// <returns>The friendly name or label for the password.</returns>
        private static string GetPasswordLabel(Operation operation)
        {
            return GetUserPasswordOperationData(operation, OperationDataConstants.NewUserPasswordFriendlyName);
        }

        /// <summary>
        /// Gets the password username from the operation data.
        /// </summary>
        /// <param name="operation">The operation to extract the password username from.</param>
        /// <returns>The username associated with the password.</returns>
        private static string GetPasswordUsername(Operation operation)
        {
            return GetUserPasswordOperationData(operation, OperationDataConstants.NewUserPasswordUsername);
        }

        /// <summary>
        /// Gets the password key from the operation data.
        /// </summary>
        /// <param name="operation">The operation to extract the password key from.</param>
        /// <returns>The password key or password itself.</returns>
        private static string GetPasswordKey(Operation operation)
        {
            return GetUserPasswordOperationData(operation, OperationDataConstants.NewUserPasswordPassword);
        }
    }
}
