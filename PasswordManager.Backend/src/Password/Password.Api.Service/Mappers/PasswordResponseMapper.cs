using PasswordManager.Password.Api.Service.Models;
using PasswordManager.Password.Domain.Password;

namespace PasswordManager.Password.Api.Service.Mappers;
internal static class PasswordResponseMapper
{
    internal static PasswordResponse Map(PasswordModel passwordModel)
    {
        var passwordResponse = new PasswordResponse(passwordModel.Id,
                                                    passwordModel.Url,
                                                    passwordModel.FriendlyName,
                                                    passwordModel.Username,
                                                    passwordModel.Password,
                                                    passwordModel.UserId);

        return passwordResponse;
    }

    internal static PasswordResponses Map(IEnumerable<PasswordModel> passwordModels)
    {
        var passwordResponses = passwordModels.Select(Map);

        return new PasswordResponses(passwordModels.Select(Map));
    }
}
