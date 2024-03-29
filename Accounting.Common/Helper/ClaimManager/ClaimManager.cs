using Microsoft.AspNetCore.Http;
using AccountingsTracker.Common.Constants;
using System.Security.Claims;

namespace Accounting.Common.Helpers
{
    public class ClaimManager : IClaimManager
    {
        private readonly IHttpContextAccessor _context;

        public ClaimManager(IHttpContextAccessor context)
        {
            _context = context;
        }

        public IEnumerable<Claim> GetClaims()
        {
            return _context.HttpContext.User.Claims ?? new List<Claim>();
        }

        public string GetEmail()
        {
            var emailClaim = GetClaims().FirstOrDefault(f => f.Type == ClaimTypes.Email);
            if (emailClaim != null)
            {
                return emailClaim.Value;
            }

            return string.Empty;
        }

        public IEnumerable<string> GetRoles()
        {
            var roleClaims = GetClaims()
                .Where(f => f.Type == ClaimTypes.Role)
                .Select(f => f.Value);

            return roleClaims;
        }
        //public string GetRole()
        //{
        //    var roleClaim = GetClaims().Select(f => f.Type == ClaimTypes.Role);
        //    if (roleClaim != null)
        //    {
        //        return roleClaim.Value;
        //    }

        //    return string.Empty;
        //}

        public Guid GetTenantId()
        {
            var tenantIdClaim = GetClaims().FirstOrDefault(f => f.Type == JwtTokenConstants.TenantId);
            if (tenantIdClaim != null)
            {
                return new Guid(tenantIdClaim.Value);
            }
            return Guid.Empty;
        }

        public Guid GetUserId()
        {
            var userIdClaim = GetClaims().FirstOrDefault(f => f.Type == JwtTokenConstants.UserId);
            if (userIdClaim != null)
            {
                return new Guid(userIdClaim.Value);
            }

            return Guid.Empty;
        }
    }
}
