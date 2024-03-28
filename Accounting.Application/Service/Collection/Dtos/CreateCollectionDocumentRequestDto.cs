using System;
using System.Collections.Generic;
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
        public string Number { get; set; }
        public DateTime LastDate { get; set; }
        public decimal Price { get; set; }
        [AllowNull]
        public Guid TenantId { get; set; }
    }
}
