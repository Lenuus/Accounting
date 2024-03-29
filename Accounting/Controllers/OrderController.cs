using Accounting.Application.Service.Order;
using Accounting.Application.Service.Order.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Policy = "ManagementRolePolicy")]
        [HttpPost("Create-Order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateRequestDto request)
        {
            var response = await _orderService.CreateOrder(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
        [Authorize(Policy = "ManagementRolePolicy")]
        [HttpPost("Delete-Order")]
        public async Task<IActionResult> DeleteOrder([FromBody] Guid id)
        {
            var response = await _orderService.RemoveOrder(id).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [Authorize(Policy = "ManagementRolePolicy")]
        [HttpPost("Update-Order")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequestDto request)
        {
            var response = await _orderService.UpdateOrder(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [Authorize(Policy = "EmployeeAndManagementPolicy")]
        [HttpPost("GetAll-Order")]
        public async Task<IActionResult> GetAllOrders([FromBody] GetAllOrderRequestDto request)
        {
            var response = await _orderService.GetAllOrder(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
