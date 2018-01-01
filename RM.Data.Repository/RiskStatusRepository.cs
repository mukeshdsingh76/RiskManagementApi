using RM.Data.Models;
using RM.Data.Repository.Contract;

namespace RM.Data.Repository
{
  public class RiskStatusRepository : Repository<RiskStatus>, IRiskStatusRepository
  {
    public RiskStatusRepository(RMContext context) : base(context)
    {
    }
  }
}