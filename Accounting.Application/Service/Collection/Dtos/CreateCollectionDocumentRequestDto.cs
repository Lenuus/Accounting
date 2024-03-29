using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Collection.Dtos
{
    public class CreateCollectionDocumentRequestDto
    {
        [AllowNull] 
        public Guid CollectionId { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public DateTime LastDate { get; set; }
        [Required]
        public decimal Price { get; set; }
        [AllowNull]
        public Guid TenantId { get; set; }
    }
}
