using Colors.Net;
using Mohaymen_sTask.Contracts;
using Colors.Net.StringColorExtensions;
using Mohaymen_sTask.Entities;

namespace Mohaymen_sTask.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationRepository _authenticationRepository;

    public AuthenticationService(IAuthenticationRepository authenticationRepository)
    {
        _authenticationRepository = authenticationRepository;
    }

    public async Task<User> Login(string username, string password, CancellationToken cancellationToken)
    {
        var user = await _authenticationRepository.Login(username, password, cancellationToken);

        if (user is not null)
        {
            ColoredConsole.WriteLine("Login was successful".Green());
            Console.ReadKey();
            return user;
        }
        else
        {
            ColoredConsole.WriteLine("Username or password is invalid".Red());
            Console.ReadKey();
            return null;
        }
    }

    public async Task<bool> Register(string username, string password, CancellationToken cancellationToken)
    {
        if (await _authenticationRepository.Register(username, password, default))
        {
            ColoredConsole.WriteLine("Registration was successful".Green());
            Console.ReadKey();
            return true;
        }
        else
        {
            ColoredConsole.WriteLine("Username already exist".Red());
            Console.ReadKey();
            return false;
        }
    }
}
