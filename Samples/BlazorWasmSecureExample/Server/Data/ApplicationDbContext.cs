using BlazorWasmSecureExample.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazorWasmSecureExample.Server.Data
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>  //ApiAuthorizationDbContext<ApplicationUser>
  {
    public ApplicationDbContext(DbContextOptions options) : base(options)
        //DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options)  //, operationalStoreOptions)
    {
    }
  }
}