﻿using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Expense.Dtos
{
    public class ExpenseListDto
    {
        public Guid Id { get; set; }
        public Guid CorporationId { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get; set; }
        public ExpenseTypeDto ExpenseType { get; set; }
        public string Description { get; set; }
    }
}
