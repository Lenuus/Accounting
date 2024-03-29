using Accounting.Application.Service.Product.Dtos;
using Accounting.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Order.Dtos
{
    public class OrderCreateRequestDto
    {
        [Required]
        public Guid CorporationId { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime LastDate { get; set; }
        public List<OrderProductDto> Products { get; set; }

    }
}
