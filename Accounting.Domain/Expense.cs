using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class Expense : IBaseEntity, ISoftDeletable, ISoftCreatable, ISoftUpdatable
    {
        public Guid Id { get; set; }
        public Guid CorporationId { get; set; }
        public Guid TenantId { get; set; }
        public Corporation Corporation { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Tax { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public ExpenseType ExpenseType { get; set; } = new ExpenseType();
        public string Description { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime InsertedDate { get; set; }
        public Guid InsertedById { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
        public DateTime DeletedDate { get; set; }
        public Guid? DeletedById { get; set; }
        public bool IsDeleted { get; set; }
    }
}
