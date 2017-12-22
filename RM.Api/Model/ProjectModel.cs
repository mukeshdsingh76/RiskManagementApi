using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Api.Model
{
  public class ProjectModel
  {
    public int Id { get; set; }
    public string ProjectCode { get; set; }
    public string Title { get; set; }
    public DateTime? StartDate { get; set; }
    public string Status { get; set; }
  }
}