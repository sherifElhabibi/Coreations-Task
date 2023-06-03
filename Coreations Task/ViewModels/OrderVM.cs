using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Coreations_Task.ViewModels
{
    public class OrderVM
    {
        [Required(ErrorMessage = "Please select a product.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please select a customer.")]
        public int CustomerId { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }
    }
}
