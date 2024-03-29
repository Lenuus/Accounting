using Accounting.Common.Enum;
using AccountingsTracker.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Product.Dtos
{
    public class GetAllProductRequestDto : PagedRequestDto
    {
        [AllowNull]
        public string Barcode { get; set; }
        [AllowNull]
        public string Number { get; set; }
        [AllowNull]
        public int? Stock { get; set; }
        [AllowNull]
        public decimal? PurchasePrice { get; set; }
        [AllowNull]
        public decimal? SellingPrice { get; set; }
        public ProductOrderBy OrderBySelect { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
