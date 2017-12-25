using RM.Data;
using RM.Data.Models;
using RM.Services.Interfaces;

namespace RM.Services.Repository
{
  public class ProjectStatusRepository : Repository<ProjectStatus>, IProjectStatusRepository
  {
    public ProjectStatusRepository(RMContext context) : base(context)
    {
    }
  }
}