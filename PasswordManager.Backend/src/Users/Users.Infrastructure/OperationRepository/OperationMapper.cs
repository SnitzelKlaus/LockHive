using PasswordManager.Users.Domain.Operations;
using System.Text.Json;

namespace PasswordManager.Users.Infrastructure.OperationRepository;
/// <summary>
/// Provides mapping functionalities between Operation and OperationEntity.
/// </summary>
internal static class OperationMapper
{
    /// <summary>
    /// Converts an Operation domain model to an OperationEntity for database persistence.
    /// </summary>
    /// <param name="operation">The operation model to convert.</param>
    /// <returns>An OperationEntity equivalent of the operation model.</returns>
    internal static OperationEntity Map(Operation operation)
    {
        var entity = new OperationEntity(operation.Id, operation.CreatedUtc, operation.ModifiedUtc,
            operation.RequestId, operation.UserId, operation.CreatedBy, operation.Name, operation.Status,
            operation.CompletedUtc,
            DictionaryToString(operation.Data));
        return entity;
    }

    /// <summary>
    /// Converts an OperationEntity from the database to an Operation domain model.
    /// </summary>
    /// <param name="operation">The operation entity to convert.</param>
    /// <returns>An Operation domain model equivalent of the operation entity.</returns>
    internal static Operation Map(OperationEntity operation)
    {
        var model = new Operation(operation.Id, operation.RequestId, operation.CreatedBy, operation.UserId,
            operation.OperationName, operation.Status,
            operation.CreatedUtc, operation.ModifiedUtc,
            operation.CompletedUtc, StringToDictionary(operation.Data));
        return model;
    }

    /// <summary>
    /// Deserializes a JSON string to a Dictionary.
    /// </summary>
    /// <param name="data">The JSON string.</param>
    /// <returns>A Dictionary representation of the string, or null if the string is null.</returns>
    private static Dictionary<string, string>? StringToDictionary(string? data)
    {
        if (data is null) return null;
        return JsonSerializer.Deserialize<Dictionary<string, string>>(data);
    }

    /// <summary>
    /// Serializes a Dictionary to a JSON string.
    /// </summary>
    /// <param name="data">The Dictionary to serialize.</param>
    /// <returns>A JSON string representation of the Dictionary, or null if the Dictionary is null.</returns>
    private static string? DictionaryToString(Dictionary<string, string>? data)
    {
        if (data is null) return null;
        return JsonSerializer.Serialize(data);
    }
}
