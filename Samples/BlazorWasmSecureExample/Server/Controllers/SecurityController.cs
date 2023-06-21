using System.Security.Claims;
using BlazorExample.Shared;
using BlazorWasmSecureExample.Server.Models;
using BlazorWasmSecureExample.Shared;
using Csla.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWasmSecureExample.Server.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class SecurityController : ControllerBase
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SecurityController(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public IEnumerable<SerializableClaim> Get()
    {
      var context = _httpContextAccessor.HttpContext;

      var principal = context?.User;
      principal ??= new ClaimsPrincipal();
      //var identity = principal?.Identity as ClaimsIdentity ?? new ClaimsIdentity();
      var claims = principal?.Identities?.SelectMany(i => i.Claims);
      return claims?.Select(c => new SerializableClaim(c)) ?? Enumerable.Empty<SerializableClaim>();
    }
  }
}