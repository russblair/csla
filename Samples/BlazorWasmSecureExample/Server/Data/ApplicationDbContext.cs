using BlazorWasmSecureExample.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazorWasmSecureExample.Server.Data
{
  public class ApplicationDbContext : DbContext  //ApiAuthorizationDbContext<ApplicationUser>
  {
    public ApplicationDbContext(DbContextOptions options) : base(options)
        //DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options)  //, operationalStoreOptions)
    {
    }
  }
}