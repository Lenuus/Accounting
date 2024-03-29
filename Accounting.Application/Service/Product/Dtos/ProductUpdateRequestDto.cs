using Accounting.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Product.Dtos
{
    public class ProductUpdateRequestDto
    {
        [Required]
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string Barcode { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Tax { get; set; }
        public decimal CurrentStock { get; set; }
        public List<ProductPropertyDto> Properties { get; set; } = new List<ProductPropertyDto>();
        public List<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
        public List<IFormFile> NewImages { get; set; } = new List<IFormFile>();
    }
}
