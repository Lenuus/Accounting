using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.AuthPolicy
{
    public class RolePolicyRequirement : IAuthorizationRequirement
    {
        public string[]? roles { get; }
        public RolePolicyRequirement(string? roles)
        {
            this.roles = roles == null ? null : roles.Split(",").ToArray<string>();
        }
    }
}
