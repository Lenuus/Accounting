﻿using Accounting.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class UserRole : IdentityUserRole<Guid>, IBaseEntity
    {
        public User User { get; set; }

        public Role Role { get; set; }
      
      
    }
}
