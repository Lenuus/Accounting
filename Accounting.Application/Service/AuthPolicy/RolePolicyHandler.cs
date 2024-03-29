using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.AuthPolicy
{
    public class RolePolicyHandler : AuthorizationHandler<RolePolicyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context
        , RolePolicyRequirement requirementVal)
        {
            if (context?.User?.Identity?.IsAuthenticated == false)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var roleClaims = context?.User.Claims.FirstOrDefault(x => x.Type == "custom_role")?.Value.ToString();
            if (roleClaims != null && (roleClaims.ToString().Split(",").ToArray().Any(requirementVal.roles.Contains)))
            {
                context?.Succeed(requirementVal);
                return Task.CompletedTask;
            }
            context?.Fail();
            return Task.CompletedTask;
        }
    }
}
