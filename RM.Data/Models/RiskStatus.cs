using System.ComponentModel.DataAnnotations;

namespace RM.Data.Models
{
  public class RiskStatus
  {
    [Key]
    public int Id { get; set; }

    public string Title { get; set; }
  }
}