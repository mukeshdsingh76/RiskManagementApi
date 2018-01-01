using System;
using IdentityServer4.Models;

namespace RM.Auth.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public ErrorMessage Error { get; internal set; }
  }
}