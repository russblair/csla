using System.Security.Claims;
using BlazorWasmSecureExample.Shared;
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
      var claims = principal?.Identities?.SelectMany(i => i.Claims);
      return claims?.Select(c => new SerializableClaim(c)) ?? Enumerable.Empty<SerializableClaim>();
    }
  }
}