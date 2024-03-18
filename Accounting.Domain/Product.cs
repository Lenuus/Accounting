using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Accounting.Domain
{
    public class Product: IBaseEntity, ISoftDeletable, ITenantEntity, ISoftCreatable, ISoftUpdatable
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string Barcode { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Tax { get; set; }
        public decimal CurrentStock { get; set; }
        public List<ProductProperty> Properties { get; set; } = new List<ProductProperty>();
        public List<ProductImage> Image { get; set; } = new List<ProductImage>();
        public List<ProductRecord> ProductRecord { get; set; }
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
