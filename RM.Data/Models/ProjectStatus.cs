using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RM.Data.Models
{
  public class ProjectStatus
  {
    [Key]
    public int Id { get; set; }

    public string Status { get; set; }
  }
}