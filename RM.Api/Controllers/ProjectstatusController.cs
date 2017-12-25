﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RM.Services.Interfaces;
using RM.Api.Model;
using RM.Data.Models;

namespace RM.Api.Controllers
{
  [Produces("application/json")]
  [Route("api/Projectstatus")]
  public class ProjectStatusController : Controller
  {
    private readonly IProjectStatusRepository _projectStatusRepository;

    public ProjectStatusController(IProjectStatusRepository projectRepository)
    {
      _projectStatusRepository = projectRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var ProjectStatusModels = new List<ProjectStatusModel>();
      try
      {
        foreach (var projectStatus in await _projectStatusRepository.GetAllAsyn().ConfigureAwait(false))
        {
          ProjectStatusModels.Add(new ProjectStatusModel()
          {
            Id = projectStatus.Id,
            Title = projectStatus.Title
          });
        }
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

      return Ok(ProjectStatusModels);
    }

    [HttpGet("{id}", Name = "GetProjectStatus")]
    public async Task<IActionResult> Get(int id)
    {
      var ProjectStatusModel = new ProjectStatusModel();

      try
      {
        var projectStatus = await _projectStatusRepository.GetAsync(id).ConfigureAwait(false);
        if (projectStatus == null)
          return NotFound();

        ProjectStatusModel.Id = projectStatus.Id;
        ProjectStatusModel.Title = projectStatus.Title;
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

      return Ok(ProjectStatusModel);
    }

    [HttpPost("create/{title}")]
    public async Task<IActionResult> Create(string title)
    {
      if (title?.Length == 0)
        return BadRequest();

      var project = await _projectStatusRepository.AddAsyn(new ProjectStatus() { Title = title }).ConfigureAwait(false);

      return CreatedAtRoute("GetProjectStatus", new { id = project.Id }, project);
    }

    [HttpPut("{id}/{title}")]
    public IActionResult Update(int id, string title)
    {
      if (id == default(int) || title?.Length == 0)
        return BadRequest();

      var projectStatus = _projectStatusRepository.Get(id);
      if (projectStatus == null)
        return NotFound();

      projectStatus.Title = title;

      _projectStatusRepository.Update(projectStatus, id);
      return new NoContentResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
      var projectToDelete = await _projectStatusRepository.GetAsync(id).ConfigureAwait(false);

      if (projectToDelete == null)
        return NotFound();

      await _projectStatusRepository.DeleteAsyn(projectToDelete).ConfigureAwait(false);

      return new NoContentResult();
    }
  }
}