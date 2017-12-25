using System;
using System.ComponentModel.DataAnnotations;

namespace RM.Data.Models
{
  public class Project
  {
    [Key]
    public int Id { get; set; }

    public string ProjectCode { get; set; }
    public string Title { get; set; }
    public DateTime? StartDate { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
  }
}