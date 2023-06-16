using BlazorExample.Shared;
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
    public string Get()
    {
      var context = _httpContextAccessor.HttpContext;

      return context?.User?.Identity?.IsAuthenticated.ToString() ?? "false";
    }
  }
}