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
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolePolicyRequirement requirementVal)
        {
            if (context?.User?.Identity?.IsAuthenticated == false)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var roleClaims = context?.User.Claims.Where(x => x.Type == "custom_role")?.Select(x => x.Value).ToList();

            if (roleClaims != null && roleClaims.Any())
            {
                foreach (var roleClaim in roleClaims)
                {
                    if (requirementVal.roles.Contains(roleClaim))
                    {
                        context.Succeed(requirementVal);
                        return Task.CompletedTask;
                    }
                }
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
