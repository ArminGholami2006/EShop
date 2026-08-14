using Basket.API.Exceptions;
using Basket.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Infrastructure.Persistence.Data;

public class BasketRepository(ApplicationContext context) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string username, CancellationToken cancellationToken = default)
    {
        var basket = await context.ShoppingCarts.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

        return basket is null
            ? throw new BasketNotFoundException(username)
            : basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        var shoppingBasket = await context.ShoppingCarts.FirstOrDefaultAsync(x => x.Username == basket.Username, cancellationToken);
        if (shoppingBasket is null)
        {
            await context.AddAsync(basket, cancellationToken);
        }
        else
        {
            shoppingBasket.Items = basket.Items;
        }

        await context.SaveChangesAsync(cancellationToken);
        return basket;
    }

    public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default)
    {
        var basket = await GetBasket(username, cancellationToken);
        context.Remove(basket);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
