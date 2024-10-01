using Microsoft.EntityFrameworkCore;
using Mohaymen_sTask.Contracts;
using Mohaymen_sTask.DataAccess;
using Mohaymen_sTask.Entities;

namespace Mohaymen_sTask.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly AppDbContext _appDbContext;

    public AuthenticationRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<User> Login(string username, string password, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password, cancellationToken);

        return user ?? null;
    }

    public async Task<bool> Register(string username, string password, CancellationToken cancellationToken)
    {
        var currentUser = await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password, cancellationToken);

        if (currentUser != null) return false;

        await _appDbContext.Users.AddAsync(new User() { UserName = username, Password = password }, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}