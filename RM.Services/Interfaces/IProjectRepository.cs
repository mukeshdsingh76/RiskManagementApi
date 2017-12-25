using RM.Data.Models;
using System.Threading.Tasks;

namespace RM.Services.Interfaces
{
  public interface IProjectRepository : IRepository<Project>
  {
    Task<Project> GetProjectWithProjectStatus(int id);
  }
}