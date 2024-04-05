using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class UserClaim : IdentityUserClaim<Guid>, IBaseEntity
    {
        public User User { get; set; }
    }
}
