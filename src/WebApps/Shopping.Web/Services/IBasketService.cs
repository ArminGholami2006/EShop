using Refit;
using Shopping.Web.Models.Basket;
using System.Net;

namespace Shopping.Web.Services;

public interface IBasketService
{
    [Get("/basket-service/basket/{username}")]
    Task<GetBasketResponse> GetBasket(string username);

    [Post("/basket-service/basket")]
    Task<StoreBasketResponse> StoreBasket(StoreBasketRequest request);

    [Delete("/basket-service/basket/{username}")]
    Task<DeleteBasketResponse> DeleteBasket(string username);

    [Post("/basket-service/basket/checkout")]
    Task<CheckoutBasketRequest> CheckoutBasket(CheckoutBasketRequest request);

    public async Task<ShoppingCartModel> LoadUserBasket()
    {
        var username = "swn";
        ShoppingCartModel basket;

        try
        {
            var getBasketResponse = await GetBasket(username);
            basket = getBasketResponse.Cart;
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            basket = new ShoppingCartModel
            {
                Username = username,
                Items = [],
            };
        }

        return basket;
    }
}
