using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RM.Data.Models
{
  public class User
  {
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string UserName { get; set; }

    [StringLength(250)]
    public string Password { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [StringLength(250)]
    public string Email { get; set; }

    [StringLength(20)]
    public string Phone { get; set; }
  }
}