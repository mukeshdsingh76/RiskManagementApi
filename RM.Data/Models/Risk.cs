using System;
using System.ComponentModel.DataAnnotations;

namespace RM.Data.Models
{
  public class Risk
  {
    [Key]
    public int Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? DateIdentified { get; set; }

    [RegularExpression(@"^\d+(.\d{1,2})?$")]
    public decimal? OptimisticEstimate { get; set; }

    [RegularExpression(@"^\d+(.\d{1,2})?$")]
    public decimal? MostLikelyEstimate { get; set; }

    [RegularExpression(@"^\d+(.\d{1,2})?$")]
    public decimal? PessimisticEstimate { get; set; }

    public virtual RiskStatus RiskStatus { get; set; }

    public virtual Project Project { get; set; }
  }
}