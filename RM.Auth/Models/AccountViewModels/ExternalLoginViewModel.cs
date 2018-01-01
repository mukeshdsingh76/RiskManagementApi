using System.ComponentModel.DataAnnotations;

namespace RM.Auth.Models.AccountViewModels
{
  public class ExternalLoginViewModel
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }
  }
}
