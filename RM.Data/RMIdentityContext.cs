using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RM.Data.Models;

namespace RM.Data
{
  public class RMIdentityContext : IdentityDbContext<User>
  {
    public RMIdentityContext(DbContextOptions option) : base(option)
    {
    }
  }
}