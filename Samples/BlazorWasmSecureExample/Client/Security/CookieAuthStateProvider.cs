using BlazorWasmSecureExample.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using Csla.Security;

namespace BlazorWasmSecureExample.Client.Security
{
  public class CookieAuthStateProvider : AuthenticationStateProvider
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CookieAuthStateProvider> _logger;
    private ClaimsPrincipal? _claimsPrincipal;

    public CookieAuthStateProvider(IHttpClientFactory httpClientFactory, ILogger<CookieAuthStateProvider> logger)
    {
      _httpClientFactory = httpClientFactory;
      _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
      IEnumerable<SerializableClaim>? claims = null;
      ClaimsIdentity? identity = null;

      if (_claimsPrincipal is not null)
      {
        return new AuthenticationState(_claimsPrincipal);
      }

      // Retrieve the claims principal from the server
      var httpClient = _httpClientFactory.CreateClient("BlazorWasmSecureExample.Server");
      try
      {
        claims = await httpClient.GetFromJsonAsync<IEnumerable<SerializableClaim>>("/security/");

        var deserializedClaims = claims?.Select(c => new Claim(
            c.Type,
            c.Value,
            c.ValueType,
            c.Issuer,
            c.OriginalIssuer
          ));

        if (deserializedClaims?.Any() ?? false)
        {
          identity = new ClaimsIdentity(deserializedClaims, "Custom");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Failed to retrieve Claims from server! Message: {Message}", ex.Message);
      }

      _claimsPrincipal = new CslaClaimsPrincipal(new ClaimsPrincipal(identity ?? new ClaimsIdentity()));
      var authenticationState = new AuthenticationState(_claimsPrincipal);

      // Notify the Blazor framework (and Csla) that the principal has changed
      NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));

      return authenticationState;
    }
  }
}