using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RM.Api.Model;
using RM.Data.Models;
using RM.Services.Interfaces;
using System.Threading.Tasks;

namespace RM.Api.Controllers
{
  [Produces("application/json")]
  [Route("api/Account")]
  public class AccountController : Controller
  {
    private IUserRepository _userRepository;
    UserManager<User> _userManager;

    public AccountController(IUserRepository userRepository, UserManager<User> userManager)
    {
      _userRepository = userRepository;
      _userManager = userManager;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
      var user = new User() { UserName = model.UserName, Email = model.Email };
      var result = await _userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        return Ok();
      }
      else
      {
        return NotFound(result.Errors);
      }

    }
  }
}