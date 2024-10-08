using Colors.Net;
using Colors.Net.StringColorExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mohaymen_sTask.Contracts;
using Mohaymen_sTask.DataAccess;
using Mohaymen_sTask.Entities;
using Mohaymen_sTask.Enums;
using Mohaymen_sTask.Repositories;
using Mohaymen_sTask.Services;

#region RegisterServices

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((services) =>
    {
        var connectionString = "Data Source=HANIE\\SQLEXPRESS; Integrated Security=True; Trust Server Certificate=True; Initial Catalog=MohaymenDB;";

        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
    });

builder.ConfigureLogging(x => x.ClearProviders());

var app = builder.Build();

#endregion


#region inject Services

var authenticationService = app.Services.GetRequiredService<IAuthenticationService>();
var userService = app.Services.GetRequiredService<IUserService>();

#endregion


User OnlineUser = new User();
bool UserIsLogin = false;
while (true)
{
    Console.Clear();
    ColoredConsole.Write("Enter Command : ".Blue());
    var input = Console.ReadLine();

    var parameters = new Dictionary<string, string>();

    var command = GetCommand(input);

    try
    {
        parameters = GetParameters(input);
    }
    catch (Exception e)
    {
        ColoredConsole.WriteLine("You entered the wrong command !".Red());
        Console.ReadKey();
        break;
    }

    switch (command)
    {
        case Command.Register:
            {
                var username = parameters["USERNAME"];
                var password = parameters["PASSWORD"];

                await authenticationService.Register(username, password, default);

                break;
            }

        case Command.Login:
            {
                if (UserIsLogin == false)
                {
                    var username = parameters["USERNAME"];
                    var password = parameters["PASSWORD"];

                    OnlineUser = await authenticationService.Login(username, password, default);
                    UserIsLogin = true;
                }
                else if(UserIsLogin == true)
                {
                    ColoredConsole.WriteLine("You can not login with another username, logout first ! ".Red());
                    Console.ReadKey();
                }
                break;
            }

        case Command.ChangePassword:
            {
                if (OnlineUser is null)
                {
                    ColoredConsole.WriteLine("To perform this operation, login first !".Red());
                    Console.ReadKey();
                }
                else
                {
                    var oldPass = parameters["OLD"];
                    var newPass = parameters["NEW"];

                    await userService.ChangePassword(OnlineUser.UserName, oldPass, newPass, default);
                }
            }
            break;

        case Command.Change:
            {
                if (OnlineUser is null)
                {
                    ColoredConsole.WriteLine("To perform this operation, login first !".Red());
                    Console.ReadKey();
                }
                else
                {
                    var status = parameters["STATUS"].ToUpper();

                    if (status == "AVAILABLE")
                    {
                        await userService.SetAvailable(OnlineUser.UserName, default);
                    }
                    else if (status == "NOTAVAILABLE")
                    {
                        await userService.SetNotAvailable(OnlineUser.UserName, default);
                    }
                }
            }
            break;

        case Command.Search:
            {
                if (OnlineUser is null)
                {
                    ColoredConsole.WriteLine("To perform this operation, login first !".Red());
                    Console.ReadKey();
                }
                else
                {
                    var username = parameters["USERNAME"];

                    var result = await userService.Search(username, default);

                    if (result == null)
                    {
                        break;
                    }
                    else
                    {
                        foreach (var user in result)
                        {
                            //ColoredConsole.WriteLine(user.Status
                            //    ? $"{user.Id}-{user.UserName}     |     status: available".DarkYellow()
                            //    : $"{user.Id}-{user.UserName}     |     status: not available".DarkYellow());

                            if (user.Status)
                                ColoredConsole.WriteLine($"{user.Id}- {user.UserName}     |     status: available".Yellow());
                            else
                                ColoredConsole.WriteLine($"{user.Id}- {user.UserName}     |     status: not available".Yellow());
                        }
                        Console.ReadKey();
                    }
                }
                break;
            }

        case Command.Delete:
            {
                if (OnlineUser is null)
                {
                    ColoredConsole.WriteLine("To perform this operation, login first !".Red());
                    Console.ReadKey();
                }
                else
                {
                    var username = parameters["USERNAME"];
                    var password = parameters["PASSWORD"];
                    if (OnlineUser.UserName == username)
                    {
                        await userService.DeleteUser(username, password, default);
                        ColoredConsole.WriteLine($"{username}, You have been successfully deleted".Green());
                        Console.ReadKey();
                    }
                    else
                    {
                        ColoredConsole.WriteLine("You can not delete another user !".Red());
                        Console.ReadKey();
                    }
                }
                break;
            }

        case Command.Update:
            {
                if (OnlineUser is null)
                {
                    ColoredConsole.WriteLine("To perform this operation, login first !".Red());
                    Console.ReadKey();
                }
                else
                {
                    var oldUserName = parameters["OLD"];
                    var newUserName = parameters["NEW"];

                    if (OnlineUser.UserName == oldUserName)
                    {
                        await userService.UpdateUserName(oldUserName, newUserName, OnlineUser.Password, default);
                    }
                    else
                    {
                        ColoredConsole.WriteLine("You can not update another user !".Red());
                        Console.ReadKey();
                    }
                }
                break;
            }

        case Command.Help:
            {
                Console.Clear();
                ColoredConsole.WriteLine("Register --username [username] --password [password]".DarkGray());
                ColoredConsole.WriteLine("Login --username [username] --password[password]".DarkGray());
                ColoredConsole.WriteLine("Change --status [available]".DarkGray());
                ColoredConsole.WriteLine("Change --status [not available]".DarkGray());
                ColoredConsole.WriteLine("Search --username [username]".DarkGray());
                ColoredConsole.WriteLine("ChangePassword --old[myOldPassword] --new[myNewPassword]".DarkGray());
                ColoredConsole.WriteLine("Update --old[myOldUsername] --new[myNewUsername]".DarkGray());
                ColoredConsole.WriteLine("Delete --username [username] --password [password]".DarkGray());
                Console.ReadKey();
                break;
            }

        case Command.Logout:
            {
                UserIsLogin = false;
                ColoredConsole.WriteLine("You are logged out of the system !".Red());
                Console.ReadKey();
                break;
            }

        case Command.Invalid:
            {
                ColoredConsole.WriteLine("You entered the wrong command !".Red());
                Console.ReadKey();
                break;
            }

        default:
            break;
    }
}

Dictionary<string, string> GetParameters(string input)
{
    var parts = input.Split(' ');

    var parameters = new Dictionary<string, string>();

    for (int i = 1; i < parts.Length; i = i + 2)
    {
        var key = parts[i].Substring(2).ToUpper();
        var value = parts[i + 1];

        if (value == "not")
        {
            value = "NOTAVAILABLE";
            parameters[key] = value;
            break;
        }
        parameters[key] = value;
    }
    return parameters;
}

Command GetCommand(string input)
{
    var inputCommand = input.Split(' ')[0].ToUpper();

    switch (inputCommand)
    {
        case "HELP":
            return Command.Help;
            break;

        case "REGISTER":
            return Command.Register;
            break;

        case "LOGIN":
            return Command.Login;
            break;

        case "CHANGEPASSWORD":
            return Command.ChangePassword;
            break;

        case "CHANGE":
            return Command.Change;
            break;

        case "SEARCH":
            return Command.Search;
            break;

        case "LOGOUT":
            return Command.Logout;
            break;

        case "DELETE":
            return Command.Delete;
            break;

        case "UPDATE":
            return Command.Update;
            break;

        default:
            return Command.Invalid;
            break;
    }
}