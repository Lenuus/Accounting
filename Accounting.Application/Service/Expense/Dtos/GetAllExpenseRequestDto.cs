using Accounting.Common.Enum;
using Accounting.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Expense.Dtos
{
    public class GetAllExpenseRequestDto : PagedRequestDto
    {
        [AllowNull]
        public string Name { get; set; }
        [AllowNull]
        public decimal Price { get; set; }
        [AllowNull]
        public decimal Tax { get; set; }
        [AllowNull]
        public decimal TotalPrice { get; set; }
        public SortDirection SortDirection { get; set; }
        public ExpenseOrderBy ExpenseOrderBy { get; set; }

    }
}
