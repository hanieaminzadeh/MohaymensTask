using Microsoft.EntityFrameworkCore;
using Mohaymen_sTask.Contracts;
using Mohaymen_sTask.DataAccess;
using Mohaymen_sTask.Dtos;

namespace Mohaymen_sTask.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task ChangePassword(string userName, string password, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);

        if (user is not null)
        {
            user.Password = password;
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> PasswordIsValid(string userName, string oldPassword, CancellationToken cancellationToken)
    {
        return await _appDbContext.Users
            .AnyAsync(x => x.UserName == userName && x.Password == oldPassword, cancellationToken);
    }

    public async Task<List<UserSearchDto>> Search(string userName, CancellationToken cancellationToken)
    {
        var users = await _appDbContext.Users
            .Where(x => x.UserName.StartsWith(userName))
            .Select(x => new UserSearchDto()
            {
                Id = x.Id,
                UserName = x.UserName,
                Status = x.IsAvailable
            }).ToListAsync(cancellationToken);

        return users;
    }

    public async Task SetAvailable(string userName, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);

        if (user is not null)
        {
            user.IsAvailable = true;
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task SetNotAvailable(string userName, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);

        if (user is not null)
        {
            user.IsAvailable = false;
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> DeleteUser(string userName, string Password, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == Password, cancellationToken);

        if (user is not null)
        {
            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        return false;
    }

    public async Task Update(string oldUserName, string newUserName, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == oldUserName, cancellationToken);

        if (user is not null)
        {
            user.UserName = newUserName;
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> UserNameIsValid(string oldUserName, string password, CancellationToken cancellationToken)
    {
        return await _appDbContext.Users
            .AnyAsync(x => x.UserName == oldUserName && x.Password == password, cancellationToken);
    }
}

