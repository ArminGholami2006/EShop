using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Basket;
using Shopping.Web.Services;

namespace Shopping.Web.Pages;

public class CheckOutModel(IBasketService basketService, ILogger<CheckOutModel> logger) : PageModel
{
    [BindProperty]
    public BasketCheckoutModel Order { get; set; } = default!;

    public ShoppingCartModel Cart { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        Cart = await basketService.LoadUserBasket();

        return Page();
    }

    public async Task<IActionResult> OnPostCheckOutAsync()
    {
        logger.LogInformation("Checkout button clicked");

        Cart = await basketService.LoadUserBasket();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        Order.CustomerId = new Guid("B5843F0C-A935-4EAA-8605-2B04D1B12933");
        Order.Username = Cart.Username;
        Order.TotalPrice = Cart.TotalPrice;

        await basketService.CheckoutBasket(new CheckoutBasketRequest(Order));

        return RedirectToPage("Confirmation", "OrderSubmitted");
    }
}
