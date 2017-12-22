using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RM.Services.Interfaces;
using RM.Api.Model;
using System;
using System.Threading.Tasks;
using RM.Data.Models;

namespace RM.Api.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class ProjectController : Controller
  {
    private readonly IProjectRepository _projectRepository;

    public ProjectController(IProjectRepository projectRepository)
    {
      _projectRepository = projectRepository;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
      var projectModels = new List<ProjectModel>();
      try
      {
        foreach (var project in await _projectRepository.GetAllAsyn().ConfigureAwait(false))
        {
          projectModels.Add(new ProjectModel()
          {
            Id = project.Id,
            ProjectCode = project.ProjectCode,
            StartDate = project.StartDate,
            Status = project.ProjectStatus?.Status,
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

    [HttpGet("{id}", Name = "Get")]
    public async Task<IActionResult> Get(int id)
    {
      var projectModel = new ProjectModel();

      try
      {
        var project = await _projectRepository.GetAsync(id).ConfigureAwait(false);
        if (project == null)
        {
          return NotFound();
        }

        projectModel.Id = project.Id;
        projectModel.ProjectCode = project.ProjectCode;
        projectModel.StartDate = project.StartDate;
        projectModel.Status = project.ProjectStatus.Status;
        projectModel.Title = project.Title;
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

      return new ObjectResult(projectModel);
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

      return CreatedAtRoute("Get", new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProjectModel model)
    {
      if (model == null)
      {
        return BadRequest();
      }

      var project = _projectRepository.Get(id);
      if (project == null)
      {
        return NotFound();
      }

      project.Title = model.Title;
      project.StartDate = model.StartDate;

      _projectRepository.Update(project, id);
      return new NoContentResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
      var projectToDelete = await _projectRepository.GetAsync(id).ConfigureAwait(false);

      if (projectToDelete == null)
      {
        return NotFound();
      }

      await _projectRepository.DeleteAsyn(projectToDelete).ConfigureAwait(false);

      return new NoContentResult();
    }
  }
}