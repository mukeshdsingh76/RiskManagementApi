using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static RM.Api.Authorization;

namespace RM.Api.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  [Authorize]
  public class IdentityController : Controller
  {
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
      if (IsRoleSatisfied(User, Role.Manager) && IsPermissionSatisfied(User, Permission.ViewClaims))
        return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
      else
        return this.Unauthorized();
    }
  }
}