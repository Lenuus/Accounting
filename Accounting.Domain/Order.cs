using Accounting.Common.Enum;
using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class Order : IBaseEntity, ISoftDeletable, ITenantEntity, ISoftCreatable, ISoftUpdatable
    {
        public Guid Id { get; set; }
        [Required]
        public Guid CorporationId { get; set; }
        public Corporation Corporation { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime LastDate { get; set; }
        [Required]
        public bool InOut { get; set; }
        [Required]
        public decimal TotalTaxAmount { get; set; } //Tax total
        [Required]
        public decimal NetPrice { get; set; }// indirim yapılmadan ve kdv eklenmemiski fiyat
        [Required]
        public decimal TotalPrice { get; set; }// indirim ve kdv uygulanmış fiyat
        [Required]
        public decimal TotalDiscount { get; set; }// indirim Totali
        [Required]
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
