using Accounting.Application;
using Accounting.Application.Service.Expense;
using Accounting.Application.Service.Expense.Dtos;
using Accounting.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseervice;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseervice = expenseService;
        }
        [Authorize(Policy = RoleClaimConstants.CorporationAdd)]
        [HttpPost("create-expense")]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseCreateRequestDto request)
        {
            var response = await _expenseervice.CreateExpense(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [Authorize(Policy = RoleClaimConstants.CorporationUpdate)]
        [HttpPost("update-expense")]
        public async Task<IActionResult> UpdateExpense([FromBody] ExpenseUpdateRequestDto request)
        {
            var response = await _expenseervice.UpdateExpense(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [Authorize(Policy = RoleClaimConstants.CorporationDelete)]
        [HttpPost("delete-expense")]
        public async Task<IActionResult> DeleteExpense([FromBody] Guid id)
        {
            var response = await _expenseervice.DeleteExpense(id).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = RoleClaimConstants.CorporationList)]
        [HttpPost("get-all-expense")]
        public async Task<IActionResult> GetAllExpenses([FromBody] GetAllExpenseRequestDto request)
        {
            var response = await _expenseervice.GetAllExpense(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
