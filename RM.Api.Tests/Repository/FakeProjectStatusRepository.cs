using RM.Data.Models;
using RM.Data.Repository.Contract;
using System.Collections.Generic;

namespace RM.Api.Tests.Repository
{
  public class FakeProjectStatusRepository : FakeRepository<ProjectStatus>, IProjectStatusRepository
  {
    static readonly object projectStatus = new List<ProjectStatus>
      {
        new ProjectStatus { Id=1, Title="New"},
        new ProjectStatus { Id=2, Title="Active"},
        new ProjectStatus { Id=3, Title="Closed"}
      };

    public FakeProjectStatusRepository() : base(projectStatus)
    {
    }
  }
}
