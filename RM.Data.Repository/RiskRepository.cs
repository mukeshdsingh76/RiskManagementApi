using RM.Data.Models;
using RM.Data.Repository.Contract;

namespace RM.Data.Repository
{
  public class RiskRepository : Repository<Risk>, IRiskRepository
  {
    public RiskRepository(RMContext context) : base(context)
    {
    }
  }
}