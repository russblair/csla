using System.Security.Claims;
using BlazorWasmSecureExample.Server.Data;
using BlazorWasmSecureExample.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Csla.Configuration;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //options.UseSqlite(connectionString));
    options.UseInMemoryDatabase(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
  options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedEmail = false;
  })
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultUI()
  .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
      options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
      options.SlidingExpiration = true;
      options.AccessDeniedPath = "/Forbidden/";
    });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor(); 
builder.Services.AddCsla(o => o
  .AddAspNetCore()
  .DataPortal(dpo => dpo
    .AddServerSideDataPortal()
    .UseLocalProxy()));

// for Mock Db
builder.Services.AddTransient(typeof(DataAccess.IPersonDal), typeof(DataAccess.Mock.PersonDal));

// If using Kestrel:
builder.Services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
// If using IIS:
builder.Services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseMigrationsEndPoint();
  app.UseWebAssemblyDebugging();
}
else
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapDefaultControllerRoute();
app.MapFallbackToFile("index.html");

app.Run();
