using Accounting.Application.Service.Product.Dtos;
using Accounting.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Order.Dtos
{
    public class UpdateOrderRequestDto
    {
        [Required]
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

        public bool InOut { get; set; }
        public List<OrderProductDto> Products { get; set; } = new List<OrderProductDto>();

    }
}
