using Accounting.Application;
using Accounting.Application.Service.Expense;
using Accounting.Application.Service.Expense.Dtos;
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

        [HttpPost("Create-Expense")]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequestDto request)
        {
            var response = await _expenseervice.CreateExpense(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Update-Expense")]
        public async Task<IActionResult> UpdateExpense([FromBody] UpdateExpenseRequestDto request)
        {
            var response = await _expenseervice.UpdateExpense(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Delete-Expense")]
        public async Task<IActionResult> DeleteExpense([FromBody] Guid id)
        {
            var response = await _expenseervice.DeleteExpense(id).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("GetAll-Expense")]
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
