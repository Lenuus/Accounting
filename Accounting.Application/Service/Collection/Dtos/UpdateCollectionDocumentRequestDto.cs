using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Collection.Dtos
{
    public class UpdateCollectionDocumentRequestDto
    {
        [Required]
        public Guid Id { get; set; }
        public Guid CollectionId { get; set; }
        public string Number { get; set; }
        public DateTime LastDate { get; set; }
        public decimal Price { get; set; }
    }
}
