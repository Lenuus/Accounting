using Accounting.Common.Enum;
using AccountingsTracker.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Product.Dtos
{
    public class GetAllProductRequestDto : PagedRequestDto
    {
        public string Barcode { get; set; }
        public string Number { get; set; }
        public int? Stock { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public ProductOrderBy OrderBySelect { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
