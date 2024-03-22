using Accounting.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Product.Dtos
{
    public class ProductCreateRequestDto
    {
        public string Number { get; set; }
        public string Barcode { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Tax { get; set; }
        public decimal CurrentStock { get; set; }
        public List<ProductPropertyDto> Properties { get; set; } = new List<ProductPropertyDto>();
        [AllowNull]
        public List<IFormFile> Images { get; set; }
    }
}
