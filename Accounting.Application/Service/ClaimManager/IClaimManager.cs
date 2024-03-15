using Accounting.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.ClaimManager
{
    public interface IClaimManager : IApplicationService
    {
        IEnumerable<Claim> GetClaims();

        string GetEmail();

        Guid GetUserId();

        Guid GetTenantId();

        string GetRole();
    }
}
