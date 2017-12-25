using RM.Data;
using RM.Data.Models;
using RM.Services.Interfaces;

namespace RM.Services.Repository
{
  public class RiskStatusRepository : Repository<RiskStatus>, IRiskStatusRepository
  {
    public RiskStatusRepository(RMContext context) : base(context)
    {
    }
  }
}