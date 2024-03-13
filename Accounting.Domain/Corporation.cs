using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.Common.Enum;
using Accounting.Domain;

namespace Accounting.Domain
{
    public class Corporation:IBaseEntity,ISoftDeletable, ITenantEntity
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public bool Title { get; set; }
        public string TCKN { get; set; }
        public string VKN { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public decimal CurrentBalance { get; set; }
        public CorporationType CorporationType { get; set; }
        public ICollection<CorporationRecord> CorporationRecord { get; set; }
        public ICollection<Order> Order { get; set; } 
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
