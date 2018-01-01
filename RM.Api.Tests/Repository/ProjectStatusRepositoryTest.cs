using Microsoft.VisualStudio.TestTools.UnitTesting;
using RM.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RM.Api.Tests.Repository
{
  [TestClass]
  public class ProjectStatusRepositoryTest
  {
    [TestMethod]
    public async Task GetAll_Should_Return_AllProjectStatus_Async()
    {
      var repo = new FakeProjectStatusRepository();
      var result = (IList<ProjectStatus>)await repo.GetAllAsynAsync(false).ConfigureAwait(false);
    }
  }
}
