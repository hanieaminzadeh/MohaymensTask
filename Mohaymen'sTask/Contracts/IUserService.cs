using Mohaymen_sTask.Dtos;

namespace Mohaymen_sTask.Contracts;

public interface IUserService
{
    Task ChangePassword(string username, string oldPassword, string newPassword ,CancellationToken cancellationToken);
    Task<List<UserSearchDto>> Search(string userName, CancellationToken cancellationToken);
    Task SetAvailable(string userName, CancellationToken cancellationToken);
    Task SetNotAvailable(string userName, CancellationToken cancellationToken);
}
