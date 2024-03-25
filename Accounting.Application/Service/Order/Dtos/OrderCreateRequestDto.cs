using Accounting.Application.Service.Product.Dtos;
using Accounting.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Order.Dtos
{
    public class OrderCreateRequestDto
    {
        public Guid CorporationId { get; set; }
        public List<OrderProductDto> Products { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastDate { get; set; }
        public decimal? NetPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalTaxAmount { get; set; }

    }
}
