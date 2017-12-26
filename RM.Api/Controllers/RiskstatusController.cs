using Microsoft.AspNetCore.Mvc;
using RM.Api.Model;
using RM.Data.Models;
using RM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RM.Api.Controllers
{
  [Produces("application/json")]
  [Route("api/riskStatus")]
  public class RiskStatusController : Controller
  {
    private readonly IRiskStatusRepository _riskStatusRepository;

    public RiskStatusController(IRiskStatusRepository riskRepository)
    {
      _riskStatusRepository = riskRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var riskStatusModels = new List<RiskStatusModel>();
      try
      {
        foreach (var riskStatus in await _riskStatusRepository.GetAllAsyn().ConfigureAwait(false))
        {
          riskStatusModels.Add(new RiskStatusModel()
          {
            Id = riskStatus.Id,
            Title = riskStatus.Title
          });
        }
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

      return Ok(riskStatusModels);
    }

    [HttpGet("{id}", Name = "GetRiskStatus")]
    public async Task<IActionResult> Get(int id)
    {
      var riskStatusModel = new RiskStatusModel();

      try
      {
        var riskStatus = await _riskStatusRepository.GetAsync(id).ConfigureAwait(false);
        if (riskStatus == null)
          return NotFound();

        riskStatusModel.Id = riskStatus.Id;
        riskStatusModel.Title = riskStatus.Title;
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

      return Ok(riskStatusModel);
    }

    [HttpPost("create/{title}")]
    public async Task<IActionResult> Create(string title)
    {
      if (title?.Length == 0)
        return BadRequest();

      var project = await _riskStatusRepository.AddAsyn(new RiskStatus() { Title = title }).ConfigureAwait(false);

      return CreatedAtRoute("GetRiskStatus", new { id = project.Id }, project);
    }

    [HttpPut("{id}/{title}")]
    public IActionResult Update(int id, string title)
    {
      if (id == default(int) || title?.Length == 0)
        return BadRequest();

      var riskStatus = _riskStatusRepository.Get(id);
      if (riskStatus == null)
        return NotFound();

      riskStatus.Title = title;

      _riskStatusRepository.Update(riskStatus, id);
      return new NoContentResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
      var projectToDelete = await _riskStatusRepository.GetAsync(id).ConfigureAwait(false);

      if (projectToDelete == null)
        return NotFound();

      await _riskStatusRepository.DeleteAsyn(projectToDelete).ConfigureAwait(false);

      return new NoContentResult();
    }
  }
}