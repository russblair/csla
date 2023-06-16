using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWasmSecureExample.Client.Security
{
  public class TestAuthStateProvider : AuthenticationStateProvider
  {
    private readonly HttpClient _httpClient;

    public TestAuthStateProvider(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
      var response = await _httpClient.GetAsync("/security/");
      var isAuthenticated = await response.Content.ReadAsStringAsync();
      var principal = bool.Parse(isAuthenticated) ? new ClaimsPrincipal(GetClaimsIdentity()) : new ClaimsPrincipal(new ClaimsIdentity());
      return new AuthenticationState(principal);
    }


    private static ClaimsIdentity GetClaimsIdentity()
    {
      var claims = new List<Claim> 
      { 
        new Claim(ClaimTypes.Name, "Test"),
        new Claim(ClaimTypes.Role, "Admin")
      };

      // To have IsAuthenticated set to true, you need to specify an authentication type in the ctor
      return new ClaimsIdentity(claims, "Custom");
    }
  }
}