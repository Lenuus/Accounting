using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class CollectionDocument : IBaseEntity, ISoftDeletable, ITenantEntity,ISoftCreatable,ISoftUpdatable
    {
        public Guid Id { get; set; }
        [Required]
        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public DateTime LastDate { get; set; }
        [Required]
        public decimal Price { get; set; }
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
