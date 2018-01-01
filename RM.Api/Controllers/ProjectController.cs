using Microsoft.AspNetCore.Mvc;
using RM.Api.Model;
using RM.Data.Models;
using RM.Data.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RM.Api.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class ProjectController : Controller
  {
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectStatusRepository _projectStatusRepository;

    public ProjectController(IProjectRepository projectRepository, IProjectStatusRepository projectStatusRepository)
    {
      _projectRepository = projectRepository;
      _projectStatusRepository = projectStatusRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var projectModels = new List<ProjectModel>();
      try
      {
        foreach (var project in await _projectRepository.GetAllAsynAsync(true).ConfigureAwait(false))
        {
          projectModels.Add(new ProjectModel()
          {
            Id = project.Id,
            ProjectCode = project.ProjectCode,
            StartDate = project.StartDate,
            Status = project.ProjectStatus?.Title,
            Title = project.Title
          });
        }
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

      return Ok(projectModels);
    }

    [HttpGet("{id}", Name = "GetProject")]
    public async Task<IActionResult> Get(int id)
    {
      var projectModel = new ProjectModel();

      try
      {
        var project = await _projectRepository.GetProjectWithProjectStatus(id).ConfigureAwait(false);
        if (project == null)
        {
          return NotFound();
        }

        projectModel.Id = project.Id;
        projectModel.ProjectCode = project.ProjectCode;
        projectModel.StartDate = project.StartDate;
        projectModel.Status = project.ProjectStatus?.Title;
        projectModel.Title = project.Title;
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

      return Ok(projectModel);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody]ProjectModel model)
    {
      var project = await _projectRepository
            .AddAsyn(new Project()
            {
              ProjectCode = model.ProjectCode,
              Title = model.Title
            })
            .ConfigureAwait(false);

      return CreatedAtRoute("GetProject", new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProjectModel model)
    {
      if (model == null)
        return BadRequest();

      var project = await _projectRepository.GetAsync(id).ConfigureAwait(false);
      if (project == null)
        return NotFound();

      project.Title = model.Title;
      project.StartDate = model.StartDate;
      project.ProjectStatus = await _projectStatusRepository.FindAsync(_ => _.Title == model.Status);

      await _projectRepository.UpdateAsyn(project, id);
      return new NoContentResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
      var projectToDelete = await _projectRepository.GetAsync(id).ConfigureAwait(false);

      if (projectToDelete == null)
        return NotFound();

      await _projectRepository.DeleteAsyn(projectToDelete).ConfigureAwait(false);

      return new NoContentResult();
    }
  }
}