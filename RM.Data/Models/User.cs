using Microsoft.AspNetCore.Identity;

namespace RM.Data.Models
{
  public class User : IdentityUser
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Designation { get; set; }
  }
}