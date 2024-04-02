using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class User : IdentityUser<Guid>, IBaseEntity, ISoftDeletable, ISoftUpdatable, ISoftCreatable
    {
        public ICollection<UserRole> Roles { get; set; }

        public Guid? TenantId { get; set; }

        #region Audit
        public DateTime InsertedDate { get; set; }
        public Guid InsertedById { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }

        public DateTime DeletedDate { get; set; }
        public Guid? DeletedById { get; set; }
        public bool IsDeleted { get; set; }
        #endregion
    }
}

