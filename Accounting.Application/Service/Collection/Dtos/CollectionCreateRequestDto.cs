using Accounting.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Collection.Dtos
{
    public class CollectionCreateRequestDto
    {
        public Guid CorporationId { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public PaymentType PaymentType { get; set; }
        [Required]
        public bool InOut { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime LastDate { get; set; }
        public List<CollectionDocumentCreateRequestDto> CollectionDocuments { get; set; } = new List<CollectionDocumentCreateRequestDto>();
    }
}
