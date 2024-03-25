using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class ProductOrder
    {
        public Guid Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid OrderId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }

}
