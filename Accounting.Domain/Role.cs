using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class Role : IdentityRole<Guid>, IBaseEntity, ISoftDeletable, ISoftCreatable, ISoftUpdatable
    {
        public ICollection<UserRole> Users { get; set; }
        public DateTime InsertedDate { get; set; }
        public Guid InsertedById { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }

        public DateTime DeletedDate { get; set; }
        public Guid? DeletedById { get; set; }
        public bool IsDeleted { get; set; }
    }
}
