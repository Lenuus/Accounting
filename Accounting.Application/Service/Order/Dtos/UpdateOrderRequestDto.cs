using Accounting.Application.Service.Product.Dtos;
using Accounting.Common.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Order.Dtos
{
    public class UpdateOrderRequestDto
    {
        public Guid Id { get; set; }
        [AllowNull]
        public Guid CorporationId { get; set; }
        public string Number { get; set; }
        [AllowNull]
        public DateTime Date { get; set; }
        [AllowNull]
        public DateTime LastDate { get; set; }
        [AllowNull]
        public ActType ActType { get; set; }
        [AllowNull]
        public decimal NetPrice { get; set; }
        [AllowNull]
        public decimal TotalPrice { get; set; }
        [AllowNull]
        public decimal TotalDiscount { get; set; }
        [AllowNull]
        public decimal TotalTaxAmount { get; set; }
        [AllowNull]
        public bool InOut { get; set; }
        public List<OrderProductDto> Products { get; set; } = new List<OrderProductDto>();

    }
}
