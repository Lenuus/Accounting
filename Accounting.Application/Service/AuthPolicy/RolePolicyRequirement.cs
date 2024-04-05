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
        public string Name { get; set; }
        public RolePolicyRequirement(string name)
        {
            Name = name;
        }
    }
}
