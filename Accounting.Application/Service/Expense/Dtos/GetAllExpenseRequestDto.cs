using Accounting.Common.Enum;
using AccountingsTracker.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Expense.Dtos
{
    public class GetAllExpenseRequestDto : PagedRequestDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get; set; }
        public SortDirection SortDirection { get; set; }

        public ExpenseOrderBy ExpenseOrderBy { get; set; }

    }
}
