using PasswordManager.Users.Domain.Operations;

/// <summary>
/// Provides helper functionalities for operations related to deleting a user's password.
/// </summary>
namespace Users.Worker.Service.DeleteUserPassword
{
    public class DeleteUserPasswordOperationHelper
    {
        /// <summary>
        /// Maps an operation to a GUID representing the user's password identifier.
        /// </summary>
        /// <param name="operation">The operation containing the user's password data.</param>
        /// <returns>A GUID that represents the user's password identifier.</returns>
        internal static Guid Map(Operation operation)
        {
            return GetPasswordId(operation);
        }

        /// <summary>
        /// Retrieves specific user password operation data based on a given operation data constant.
        /// </summary>
        /// <param name="operation">The operation from which to extract the data.</param>
        /// <param name="operationDataConstant">The constant used to identify the specific piece of data to retrieve.</param>
        /// <returns>The data associated with the specified operation data constant.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the specified operation data cannot be found.</exception>
        private static string GetUserPasswordOperationData(Operation operation, string operationDataConstant)
        {
            if (operation.Data is null || operation.Data.TryGetValue(operationDataConstant, out var getPasswordOperationData) is false)
                throw new InvalidOperationException($"Could not find user password {operationDataConstant} in operation with request id {operation.RequestId} when creating user password");

            return getPasswordOperationData;
        }

        /// <summary>
        /// Extracts and parses the GUID representing the user's password identifier from an operation.
        /// </summary>
        /// <param name="operation">The operation containing the user's password identifier.</param>
        /// <returns>A GUID representing the user's password identifier.</returns>
        private static Guid GetPasswordId(Operation operation)
        {
            var getPasswordId = GetUserPasswordOperationData(operation, OperationDataConstants.UserPasswordId);
            return Guid.Parse(getPasswordId);
        }
    }
}
