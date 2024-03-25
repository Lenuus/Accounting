using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Accounting.Domain
{
    public class Product : IBaseEntity, ISoftDeletable, ITenantEntity, ISoftCreatable, ISoftUpdatable
    {
        public Guid Id { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        public decimal PurchasePrice { get; set; }
        [Required]
        public decimal SellingPrice { get; set; }
        [Required]
        public decimal Tax { get; set; }
        [Required]
        public decimal CurrentStock { get; set; }
        public List<ProductProperty> Properties { get; set; } = new List<ProductProperty>();
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
        public List<ProductRecord> ProductRecords { get; set; } = new List<ProductRecord>();
        public List<ProductOrder> Orders { get; set; }
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
