using PasswordManager.Users.Domain.Operations;

namespace PasswordManager.Users.Api.Service.Endpoints.GetOperation;
public record OperationResponse(string RequestId, Guid CustomerId, OperationName OperationName,
    OperationStatus OperationStatus, string CreatedBy, DateTime CreatedUtc, DateTime? LastModifiedUtc,
    DateTime? CompletedUtc, Dictionary<string, string>? Data);

public static class OperationMapper
{
    public static OperationResponse ToResponseModel(Operation operation)
    {
        return new OperationResponse(
            operation.RequestId,
            operation.UserId,
            operation.Name,
            operation.Status,
            operation.CreatedBy,
            operation.CreatedUtc,
            operation.ModifiedUtc,
            operation.CompletedUtc,
            operation.Data
        );
    }
}
