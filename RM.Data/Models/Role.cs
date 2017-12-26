using Microsoft.AspNetCore.Identity;

namespace RM.Data.Models
{
  public class Role : IdentityRole
  {
    public string RoleName { get; set; }
  }
}