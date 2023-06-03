using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Coreations_Task.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        public ProductController(IProductService productService, ICustomerService customerService)
        {
            _productService = productService;
            _customerService = customerService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetAll(a=>a.Customer));
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) { return View("NotFound"); }
            return View(product);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            await _productService.AddAsync(product);
            return RedirectToAction(nameof(Index));

        }
        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var product = await _productService.GetByIdAsync(id);
        //    if (product == null) { return View("NotFound"); }
        //    return View(product);


        //}
        //[HttpPost]
        //public async Task<IActionResult> Edit(Product product, int id)
        //{
        //    if (id != product.Id) { return View("NotFound"); }
        //    if (!ModelState.IsValid) { return View(product); }
        //    await _productService.UpdateAsync(id, product);
        //    return RedirectToAction(nameof(Index));

        //}
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _productService.GetByIdAsync(id);
            if (actor == null) return View("NotFound");
            return View(actor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var actor = await _productService.GetByIdAsync(id);
            if (actor == null)
            {
                return View(actor);
            }

            await _productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
