using Mohaymen_sTask.Entities;

namespace Mohaymen_sTask.Contracts;

public interface IAuthenticationService
{
    Task<User> Login(string username, string password, CancellationToken cancellationToken);
    Task<bool> Register(string username, string password, CancellationToken cancellationToken);
}
