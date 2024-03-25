using Accounting.Common.Enum;
using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Collection.Dtos
{
    public class CollectionListDto
    {
        public Guid Id { get; set; }
        public Guid CorporationId { get; set; }
        public string Number { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool InOut { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastDate { get; set; }
        public Guid TenantId { get; set; }
        public List<CollectionDocumentListDto> CollectionDocuments { get; set; } = new List<CollectionDocumentListDto>();    
    }
}
