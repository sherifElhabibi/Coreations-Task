using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coreations_Task.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<IActionResult> OrderHistory()
        {
            var orders = await _orderService.GetAll(a=>a.Products,a=>a.Customers);
            return View(orders);
        }
    }
}
