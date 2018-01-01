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
  [Route("api/Risk")]
  public class RiskController : Controller
  {
    private readonly IRiskRepository _riskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IRiskStatusRepository _riskStatusRepository;

    public RiskController(IRiskRepository riskRepository, IProjectRepository projectRepository, IRiskStatusRepository riskStatusRepository)
    {
      _riskRepository = riskRepository;
      _projectRepository = projectRepository;
      _riskStatusRepository = riskStatusRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var RiskModels = new List<RiskModel>();
      try
      {
        foreach (var risk in await _riskRepository.GetAllAsynAsync().ConfigureAwait(false))
        {
          RiskModels.Add(new RiskModel()
          {
            Id = risk.Id,
            Title = risk.Title,
            Description = risk.Description,
            DateIdentified = risk.DateIdentified,
            MostLikelyEstimate = risk.MostLikelyEstimate,
            OptimisticEstimate = risk.OptimisticEstimate,
            PessimisticEstimate = risk.PessimisticEstimate,
            Status = risk.RiskStatus?.Title,
            ProjectId = risk.Project.Id > default(int) ? _projectRepository.GetAsync(risk.Project.Id).Id : default(int)
          });
        }
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

      return Ok(RiskModels);
    }

    [HttpGet("{id}", Name = "GetRisk")]
    public async Task<IActionResult> Get(int id)
    {
      var riskModel = new RiskModel();

      try
      {
        var risk = await _riskRepository.GetAsync(id).ConfigureAwait(false);
        if (risk == null)
        {
          return NotFound();
        }

        riskModel.Id = risk.Id;
        riskModel.Title = risk.Title;
        riskModel.Description = risk.Description;
        riskModel.DateIdentified = risk.DateIdentified;
        riskModel.MostLikelyEstimate = risk.MostLikelyEstimate;
        riskModel.OptimisticEstimate = risk.OptimisticEstimate;
        riskModel.PessimisticEstimate = risk.PessimisticEstimate;
        riskModel.Status = risk.RiskStatus?.Title;
        riskModel.ProjectId = risk.Project?.Id ?? 0;
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

      return Ok(riskModel);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody]RiskModel model)
    {
      var project = await _projectRepository.GetAsync(model.ProjectId);
      if (project == null)
        return NotFound("Project not found");

      var riskStatus = await _riskStatusRepository.FindAsync(_ => _.Title == model.Status);
      if (riskStatus == null)
        return NotFound("Risk Status not found");

      var risk = await _riskRepository
            .AddAsyn(new Risk()
            {
              Title = model.Title,
              Description = model.Description,
              DateIdentified = model.DateIdentified,
              MostLikelyEstimate = model.MostLikelyEstimate,
              OptimisticEstimate = model.OptimisticEstimate,
              PessimisticEstimate = model.PessimisticEstimate,
              RiskStatus = riskStatus,
              Project = project
            })
            .ConfigureAwait(false);

      return CreatedAtRoute("GetRisk", new { id = risk.Id }, risk);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] RiskModel model)
    {
      if (model == null)
        return BadRequest();

      var risk = await _riskRepository.GetAsync(id);
      if (risk == null)
        return NotFound();

      risk.Title = model.Title;
      risk.Description = model.Description;
      risk.DateIdentified = model.DateIdentified;
      risk.MostLikelyEstimate = model.MostLikelyEstimate;
      risk.OptimisticEstimate = model.OptimisticEstimate;
      risk.PessimisticEstimate = model.PessimisticEstimate;
      risk.RiskStatus = await _riskStatusRepository.FindAsync(_ => _.Title == model.Status);
      risk.Project = await _projectRepository.FindAsync(_ => _.Id == model.ProjectId);

      await _riskRepository.UpdateAsyn(risk, id);
      return new NoContentResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
      var projectToDelete = await _riskRepository.GetAsync(id).ConfigureAwait(false);

      if (projectToDelete == null)
      {
        return NotFound();
      }

      await _riskRepository.DeleteAsyn(projectToDelete).ConfigureAwait(false);

      return new NoContentResult();
    }
  }
}