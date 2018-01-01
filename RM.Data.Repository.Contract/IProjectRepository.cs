using RM.Data.Models;
using System.Threading.Tasks;

namespace RM.Data.Repository.Contract
{
  public interface IProjectRepository : IRepository<Project>
  {
    Task<Project> GetProjectWithProjectStatus(int id);
  }
}