using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class Expenses
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public string Description { get; set; }

    }
}
