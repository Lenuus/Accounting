using Accounting.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Product.Dtos
{
    public class ProductCreateRequestDto
    {
        [Required]
        public string Number { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        public decimal PurchasePrice { get; set; }
        [Required]
        public decimal SellingPrice { get; set; }
        [Required]
        public decimal Tax { get; set; }
        [Required]
        public decimal CurrentStock { get; set; }
        public List<ProductPropertyDto> Properties { get; set; } = new List<ProductPropertyDto>();
        [AllowNull]
        public List<IFormFile> Images { get; set; }
    }
}
