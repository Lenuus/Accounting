using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.Common.Enum;
using Accounting.Domain;

namespace Accounting.Domain
{
    public class Corporation : IBaseEntity, ISoftDeletable, ITenantEntity, ISoftCreatable, ISoftUpdatable
    {
        public Guid Id { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public bool Title { get; set; }
        public string TCKN { get; set; }
        public string VKN { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public decimal CurrentBalance { get; set; }
        [Required]
        public CorporationType CorporationType { get; set; }
        public ICollection<CorporationRecord> CorporationRecords { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public ICollection<Collection> Collections { get; set; }
        public ICollection<Order> Orders { get; set; }
        [Required]
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
