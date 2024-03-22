using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class CollectionDocument : IBaseEntity, ISoftDeletable, ITenantEntity,ISoftCreatable,ISoftUpdatable
    {
        public Guid Id { get; set; }
        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }
        public string Number { get; set; }
        public DateTime LastDate { get; set; }
        public decimal Price { get; set; }
        public Guid TenantId { get; set; }

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
