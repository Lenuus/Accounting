using Accounting.Application.Service.Expense.Dtos;
using Accounting.Common.Enum;
using Accounting.Common.Helpers;
using AccountingsTracker.Common.Dtos;
using AccountingsTracker.Common.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Accounting.Application.Service.Expense
{
    public class ExpenseService : IExpenseService
    {
        private readonly IRepository<Domain.Expense> _expense;
        private readonly IRepository<Domain.ExpenseType> _expenseType;
        private readonly IMapper _mapper;
        private readonly IClaimManager _claimManager;


        public ExpenseService(IMapper mapper, IRepository<Domain.ExpenseType> expenseType, IRepository<Domain.Expense> expenses, IClaimManager claimManager)
        {
            _mapper = mapper;
            _expenseType = expenseType;
            _expense = expenses;
            _claimManager = claimManager;
        }

        public async Task<ServiceResponse> CreateExpense(CreateExpenseRequestDto request)
        {
            if (request == null)
            {
                return new ServiceResponse(false, "Request is not valid");
            }
            var mappingExpense = _mapper.Map<Domain.Expense>(request);
            mappingExpense.TotalPrice = mappingExpense.Price + mappingExpense.Tax;
            mappingExpense.ExpenseType.Name = request.ExpenseType.Name;
            mappingExpense.TenantId = _claimManager.GetTenantId();
            await _expense.Create(mappingExpense).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse> DeleteExpense(Guid id)
        {

            var expense = await _expense.GetById(id).ConfigureAwait(false);
            if (expense == null)
            {
                new ServiceResponse(false, "Expense can not found");
            }
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _expense.DeleteById(id).ConfigureAwait(false);
                    await _expenseType.DeleteById(expense.ExpenseType.Id).ConfigureAwait(false);

                    scope.Complete();
                }
                catch (Exception)
                {

                    return new ServiceResponse(false, "Expense Could not deleted");
                }
            }

            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse<PagedResponseDto<ExpenseListDto>>> GetAllExpense(GetAllExpenseRequestDto request)
        {
            var loggedTenantId = _claimManager.GetTenantId();
            var query = _expense.GetAll().Include(f => f.ExpenseType).Where(f => !f.IsDeleted && f.TenantId == loggedTenantId &&
                !string.IsNullOrEmpty(request.Name) ? f.Name.Contains(request.Name) : true);
            if (request.ExpenseOrderBy == ExpenseOrderBy.TotalPrice)
            {
                if (request.SortDirection == SortDirection.Ascending)
                    query = query.OrderBy(f => f.TotalPrice);
                else
                    query = query.OrderByDescending(f => f.TotalPrice);
            }
            else if (request.ExpenseOrderBy == ExpenseOrderBy.Tax)
            {
                if (request.SortDirection == SortDirection.Ascending)
                    query = query.OrderBy(f => f.Tax);
                else
                    query = query.OrderByDescending(f => f.Tax);
            }
            else if (request.ExpenseOrderBy == ExpenseOrderBy.Price)
            {
                if (request.SortDirection == SortDirection.Ascending)
                    query = query.OrderBy(f => f.Price);
                else
                    query = query.OrderByDescending(f => f.Price);
            }
            var expensesList = await query.Select(f => new ExpenseListDto
            {
                Id = f.Id,
                CorporationId = f.CorporationId,
                Description = f.Description,
                Price = f.Price,
                Tax = f.Tax,
                TotalPrice = f.TotalPrice,
                ExpenseType = new ExpenseTypeDto
                {
                    Name = f.ExpenseType.Name,

                },

            }).ToPagedListAsync(request.PageSize, request.PageSize).ConfigureAwait(false);

            return new ServiceResponse<PagedResponseDto<ExpenseListDto>>(expensesList, true, string.Empty);
        }

        public async Task<ServiceResponse> UpdateExpense(UpdateExpenseRequestDto request)
        {
            var expense = await _expense.GetById(request.Id).ConfigureAwait(false);
            expense.Name = request.Name;
            expense.CorporationId = request.CorporationId;
            expense.Price = request.Price;
            expense.Tax = request.Tax;
            expense.TotalPrice = expense.Price + expense.Tax;
            expense.ExpenseType.Name = request.ExpenseType.Name;
            await _expense.Update(expense).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }
    }
}
