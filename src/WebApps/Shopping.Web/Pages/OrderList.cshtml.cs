using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Ordering;
using Shopping.Web.Services;

namespace Shopping.Web.Pages;

public class OrderListModel(IOrderingService orderingService, ILogger<OrderListModel> logger) : PageModel
{
    public IEnumerable<OrderModel> Orders { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var customerId = new Guid("B5843F0C-A935-4EAA-8605-2B04D1B12933");

        var response = await orderingService.GetOrdersByCustomer(customerId);
        Orders = response.Orders;

        return Page();
    }
}
