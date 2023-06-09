using BlazorWasmSecureExample.Client;
using BlazorWasmSecureExample.Client.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("BlazorWasmSecureExample.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
    //.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorWasmSecureExample.ServerAPI"));

//builder.Services.AddApiAuthorization();
//builder.Services.AddRemoteAuthentication<object, object>();

builder.Services.AddScoped<AuthenticationStateProvider, TestAuthStateProvider>();

//// configure CSLA Authentication and authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddOptions();

//builder.Services.AddCsla(o => o
//  .AddBlazorWebAssembly()
//  .DataPortal(dpo => dpo
//    .EnableSecurityPrincipalFlowFromClient()
//    .UseHttpProxy(options => options.DataPortalUrl = "/api/DataPortal")));

await builder.Build().RunAsync();
