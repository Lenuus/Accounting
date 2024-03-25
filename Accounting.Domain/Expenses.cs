using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class Expenses
    {
        public Guid Id { get; set; }
        public Guid CorporationId { get; set; }
        public Corporation Corporation { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Tax { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public ExpenseType ExpenseType { get; set; }
        public string Description { get; set; }

    }
}
