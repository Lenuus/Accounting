using Accounting.Application.Service.Expense.Dtos;
using Accounting.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Expense.Mapping
{
    public class ExpenseMapper : Profile
    {
        public ExpenseMapper()
        {
            CreateMap<CreateExpenseRequestDto, Domain.Expense>().ForMember(dest => dest.ExpenseType, opt => opt.Ignore());
        }
    }
}
