using BlazorWasmSecureExample.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace BlazorWasmSecureExample.Server.Extensions
{
  internal static class WebApplicationExtensions
  {
    internal static async Task<WebApplication> AddDevelopmentSecurityData(this WebApplication webApplication)
    {
      await CreateAdminUser(webApplication);

      return webApplication;
    }

    private static async Task CreateAdminUser(WebApplication webApplication)
    {
      var adminUser = new ApplicationUser
      {
        Id = "Admin",
        Email = "admin@example.com",
        UserName = "admin@example.com"
      };

      // create a scope to allow access to scoped services
      using var scope = webApplication.Services.CreateScope();
      var services = scope.ServiceProvider;

      // create user
      var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();
      _ = await userMgr.CreateAsync(adminUser);

      // add password
      _ = await userMgr.AddPasswordAsync(adminUser, "Admin123!");

      // add admin role to admin user
      var adminRole = await CreateAdminRole(services);
      await userMgr.AddToRoleAsync(adminUser, adminRole.Name!);
    }

    private static async Task<ApplicationRole> CreateAdminRole(IServiceProvider services)
    {
      var adminRole = new ApplicationRole
      {
        Id = "Admin",
        Name = "Admin"
      }; 
      
      // create role
      var roleMgr = services.GetRequiredService<RoleManager<ApplicationRole>>();
      _ = await roleMgr.CreateAsync(adminRole);

      return adminRole;
    }
  }
}
