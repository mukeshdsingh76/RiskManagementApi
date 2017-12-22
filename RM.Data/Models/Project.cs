using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RM.Data.Models
{
  public class Project
  {
    [Key]
    public int Id { get; set; }

    public string ProjectCode { get; set; }
    public string Title { get; set; }
    public DateTime? StartDate { get; set; }
    public virtual ProjectStatus ProjectStatus { get; set; }
  }
}