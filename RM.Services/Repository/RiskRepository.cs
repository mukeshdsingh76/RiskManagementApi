using RM.Data;
using RM.Data.Models;
using RM.Services.Interfaces;

namespace RM.Services.Repository
{
  public class RiskRepository : Repository<Risk>, IRiskRepository
  {
    public RiskRepository(RMContext context) : base(context)
    {
    }
  }
}