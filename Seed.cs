using Microsoft.AspNetCore.Identity;
using Queue.Models;

namespace register;

public class Seed : BackgroundService
{
    private UserManager<AdminModel> _userM;
    private RoleManager<IdentityRole> _roleM;
    private readonly IServiceProvider _provider;

    public Seed(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        _userM = scope.ServiceProvider.GetRequiredService<UserManager<AdminModel>>();
        _roleM = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new []{ "superadmin",  "user" };
        foreach(var role in roles)
        {
            if(!await _roleM.RoleExistsAsync(role))
            {
                await _roleM.CreateAsync(new IdentityRole(role));
            }
        }

        if((await _userM.FindByNameAsync("Super Admin")) == null)
        {
            var user = new AdminModel()
            {
                Fullname = "Super Admin",
                Password="123456"
            };

            var result = await _userM.CreateAsync(user, "123456");
            if(result.Succeeded)
            {
                var newUser = await _userM.FindByEmailAsync("superadmin@ilmhub.uz");
                await _userM.AddToRoleAsync(newUser, "superadmin");
            }
        }

    }
}