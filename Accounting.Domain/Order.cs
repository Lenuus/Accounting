using Accounting.Common.Enum;
using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class Order : IBaseEntity, ISoftDeletable, ITenantEntity, ISoftCreatable, ISoftUpdatable
    {
        public Guid Id { get; set; }
        public Guid CorporationId { get; set; }
        public Corporation Corporation { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastDate { get; set; }
        public ActType ActType { get; set; }
        public decimal TotalTaxAmount { get; set; } //Tax total
        public decimal NetPrice { get; set; }// indirim yapılmadan ve kdv eklenmemiski fiyat
        public decimal TotalPrice { get; set; }// indirim ve kdv uygulanmış fiyat
        public decimal TotalDiscount { get; set; }// indirim Totali
        public Guid TenantId { get; set; }
        public List<ProductOrder> Products { get; set; }
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
