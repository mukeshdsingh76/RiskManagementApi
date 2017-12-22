using RM.Data;
using RM.Data.Models;
using RM.Services.Interfaces;

namespace RM.Services.Repository
{
  public class ProjectRepository : Repository<Project>, IProjectRepository
  {
    public ProjectRepository(RMContext context) : base(context)
    {
    }
  }
}