using RM.Data.Models;
using RM.Data.Repository.Contract;

namespace RM.Data.Repository
{
  public class ProjectStatusRepository : Repository<ProjectStatus>, IProjectStatusRepository
  {
    public ProjectStatusRepository(RMContext context) : base(context)
    {
    }
  }
}