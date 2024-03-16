using Accounting.Common.Enum;
using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class Collection : IBaseEntity, ISoftDeletable, ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid CorporationId { get; set; }
        public Corporation Corporation { get; set; }
        public string Number { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool InOut { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastDate { get; set; }
        public Guid TenantId { get; set; }

        public ICollection<CollectionDocument> CollectionDocuments { get; set; }

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
