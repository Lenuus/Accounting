using Accounting.Application.Service.Collection.Dtos;
using Accounting.Application.Service.Expense.Dtos;
using AccountingsTracker.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Expense
{
    public interface IExpenseService : IApplicationService
    {
        Task<ServiceResponse> CreateExpense(CreateExpenseRequestDto request);
        Task<ServiceResponse> DeleteExpense(Guid id);
        Task<ServiceResponse> UpdateExpense(UpdateExpenseRequestDto request);
        Task<ServiceResponse<PagedResponseDto<ExpenseListDto>>> GetAllExpense(GetAllExpenseRequestDto request);
    }
}
