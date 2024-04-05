using Accounting.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.AuthPolicy
{
    public class RolePolicyHandler : AuthorizationHandler<RolePolicyRequirement>
    {
        private readonly IClaimManager _claimManager;
        private readonly IMemoryCache _memoryCache;

        public RolePolicyHandler(
            IClaimManager claimManager, 
            IMemoryCache memoryCache)
        {
            _claimManager = claimManager;
            _memoryCache = memoryCache;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolePolicyRequirement requirementVal)
        {
            if (context?.User?.Identity?.IsAuthenticated == false)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var userId = _claimManager.GetUserId();
            if (userId != Guid.Empty)
            {
                var claims = _memoryCache.Get<List<string>>($"claims_{userId}");
                if (claims != null && claims.Any() && claims.Any(f => f == requirementVal.Name))
                {
                    context.Succeed(requirementVal);
                    return Task.CompletedTask;
                }
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
