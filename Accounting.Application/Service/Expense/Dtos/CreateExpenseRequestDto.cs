using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Expense.Dtos
{
    public class CreateExpenseRequestDto
    {
        [AllowNull]
        public Guid CorporationId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Tax { get; set; }
        [Required]
        public ExpenseTypeDto ExpenseType { get; set; } = new ExpenseTypeDto();
        public string Description { get; set; }
    }
}
