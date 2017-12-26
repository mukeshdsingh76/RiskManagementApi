using Microsoft.AspNetCore.Identity;

namespace RM.Data.Models
{
  public class User : IdentityUser
  {
    public string Designation { get; set; }
  }
}