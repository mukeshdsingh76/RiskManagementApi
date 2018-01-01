using RM.Data.Models;
using RM.Data.Repository.Contract;

namespace RM.Data.Repository
{
  public class UserRepository : Repository<User>, IUserRepository
  {
    public UserRepository(RMContext context) : base(context)
    {
    }
  }
}