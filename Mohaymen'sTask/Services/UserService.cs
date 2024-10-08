using Colors.Net.StringColorExtensions;
using Colors.Net;
using Mohaymen_sTask.Contracts;
using Mohaymen_sTask.Dtos;

namespace Mohaymen_sTask.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ChangePassword(string username, string oldPassword, string newPassword, CancellationToken cancellationToken)
    {
        if (await _userRepository.PasswordIsValid(username, oldPassword, cancellationToken))
        {
            await _userRepository.ChangePassword(username, newPassword, cancellationToken);
            ColoredConsole.WriteLine("Your password has been changed".Green());
            Console.ReadKey();
        }
        else
        {
            ColoredConsole.WriteLine("The password entered is not invalid".Red());
            Console.ReadKey();
        }
    }

    public async Task<List<UserSearchDto>> Search(string userName, CancellationToken cancellationToken)
    {
        var result = await _userRepository.Search(userName, cancellationToken);

        if (result.Any())
            return result;
        else
        {
            ColoredConsole.WriteLine($"No user starts with keyword '{userName}'".Red());
            Console.ReadKey();
            return null;
        }
    }

    public async Task SetAvailable(string userName, CancellationToken cancellationToken)
    {
        await _userRepository.SetAvailable(userName, cancellationToken);
        ColoredConsole.WriteLine($"User with username '{userName}' set available ".Green());
        Console.ReadKey();
    }

    public async Task SetNotAvailable(string userName, CancellationToken cancellationToken)
    {
        await _userRepository.SetNotAvailable(userName, cancellationToken);
        ColoredConsole.WriteLine($"User with username '{userName}' set not available ".Red());
        Console.ReadKey();
    }

    public async Task<bool> DeleteUser(string userName, string Password, CancellationToken cancellationToken)
    {
        var user = await _userRepository.DeleteUser(userName, Password, cancellationToken);

        if (user)
        {
            ColoredConsole.WriteLine($"{userName} is deleted".Green());
            Console.ReadKey();
            return true;
        }
        else
        {
            ColoredConsole.WriteLine($"There is No '{userName}' user !".Red());
            Console.ReadKey();
            return false;
        }
    }

    public async Task UpdateUserName(string oldUserName, string newUserName, string password, CancellationToken cancellationToken)
    {
        if (await _userRepository.UserNameIsValid(oldUserName, password, cancellationToken))
        {
            await _userRepository.Update(oldUserName, newUserName, cancellationToken);
            ColoredConsole.WriteLine("Your user name has been updated".Green());
            Console.ReadKey();
        }
        else
        {
            ColoredConsole.WriteLine("The username entered is not invalid".Red());
            Console.ReadKey();
        }
    }
}