using RM.Data;
using RM.Data.Models;
using RM.Services.Interfaces;

namespace RM.Services.Repository
{
  public class UserRepository : Repository<User>, IUserRepository
  {
    public UserRepository(RMContext context) : base(context)
    {
    }
  }
}