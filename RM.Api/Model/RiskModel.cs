using System;

namespace RM.Api.Model
{
  public class RiskModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? DateIdentified { get; set; }
    public decimal? OptimisticEstimate { get; set; }
    public decimal? MostLikelyEstimate { get; set; }
    public decimal? PessimisticEstimate { get; set; }
    public string Status { get; set; }
    public int ProjectId { get; set; }
  }
}