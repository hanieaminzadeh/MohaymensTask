using Mohaymen_sTask.Dtos;

namespace Mohaymen_sTask.Contracts;

public interface IUserRepository
{
    Task ChangePassword(string userName, string password, CancellationToken cancellationToken);
    Task<bool> PasswordIsValid(string userName, string oldPassword, CancellationToken cancellationToken);
    Task<List<UserSearchDto>> Search(string userName, CancellationToken cancellationToken);
    Task SetAvailable(string userName , CancellationToken cancellationToken);
    Task SetNotAvailable(string userName , CancellationToken cancellationToken);
}