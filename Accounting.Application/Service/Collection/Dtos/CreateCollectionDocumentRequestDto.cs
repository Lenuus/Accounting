using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Collection.Dtos
{
    public class CreateCollectionDocumentRequestDto
    {
        public Guid CollectionId { get; set; }
        public string Number { get; set; }
        public DateTime LastDate { get; set; }
        public decimal Price { get; set; }
        public Guid TenantId { get; set; }
    }
}
