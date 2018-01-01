using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RM.Api.Controllers;
using RM.Data.Models;
using RM.Data.Repository.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RM.Api.Tests.UnitTest.Controller
{
  [TestClass]
  public class ProjectStatusControllerTests
  {
    public readonly IProjectStatusRepository MockProjectStatusRepository;

    public ProjectStatusControllerTests()
    {
      var projectStatus = new List<ProjectStatus>
      {
        new ProjectStatus { Id=1, Title="New"},
        new ProjectStatus { Id=2, Title="Active"},
        new ProjectStatus { Id=3, Title="Closed"}
      };

      var mockProjectStatusRepository = new Mock<IProjectStatusRepository>();
      mockProjectStatusRepository.Setup(mr => mr.GetAllAsynAsync(false)).ReturnsAsync(projectStatus);

      this.MockProjectStatusRepository = mockProjectStatusRepository.Object;
    }

    [TestMethod]
    public async Task ProjectStatusController_Get_Should_Return_ProjectStatusListAsync()
    {
      var controller = new ProjectStatusController(MockProjectStatusRepository);

      var result = await controller.Get().ConfigureAwait(false);

      var contentResult = result as OkObjectResult;

      //var contentResult = result as ContentResult;
      //controller.ControllerContext = new ControllerContext
      //{
      //  //HttpContext = new DefaultHttpContext
      //  //{
      //  //  User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
      //  //   new Claim( ClaimTypes.Name, System.Guid.NewGuid().ToString())
      //  //}))
      //  //}
      //};

      contentResult.Should().NotBeNull();
    }
  }
}
