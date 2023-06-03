using Core.Entities;
using Core.Interfaces;
using Coreations_Task.ViewModels;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Coreations_Task.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        public CustomerController(ICustomerService customerService , IProductService productService, IOrderService orderService)
        {
            _customerService = customerService;
            _productService = productService;
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _customerService.GetAll());
        }
        [HttpGet]
        public async Task<IActionResult> CreateOrder(int customerId)
        {
            var products = await _productService.GetAll();

            var model = new OrderVM
            {
                CustomerId = customerId,
                Products = new SelectList(products, "Id", "Name")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderVM model)
        {
            if (ModelState.IsValid)
            {
                var order = new Order
                {
                    ProductId = model.ProductId,
                    CustomerId = model.CustomerId,
                    OrderDate = DateTime.Now
                };

                await _orderService.AddAsync(order);

                return RedirectToAction("OrderHistory", "Customer", new { id = model.CustomerId });
            }

            // If the model is not valid, redisplay the create order form
            model.Products = new SelectList(await _productService.GetAll(), "Id", "Name");
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }
            await _customerService.AddAsync(customer);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null) { return View("NotFound"); }
            return View(customer);


        }
        [HttpPost]
        public async Task<IActionResult> Edit(Customer customer, int id)
        {
            if (id != customer.Id) { return View("NotFound"); }
            if (!ModelState.IsValid) { return View(customer); }
            await _customerService.UpdateAsync(id, customer);
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null) { return View("NotFound"); }
            return View(customer);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _customerService.GetByIdAsync(id);
            if (actor == null) return View("NotFound");
            return View(actor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var actor = await _customerService.GetByIdAsync(id);
            if (actor == null)
            {
                return View(actor);
            }

            await _customerService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
