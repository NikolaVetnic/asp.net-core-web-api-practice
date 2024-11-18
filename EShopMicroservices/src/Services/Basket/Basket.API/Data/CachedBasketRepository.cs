using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Api.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache)
    : IBasketRepository
{
    /*
     * CachedBasketRepository acts as a proxy which forwards all the c-
     * alls to underlying IBasketRepository; in addition, the function-
     * ality of the IBasketRepository is extended with caching by means
     * of Decorator pattern.
     */

    public async Task<ShoppingCart> GetBasket(string username, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(username, cancellationToken);

        if (!string.IsNullOrWhiteSpace(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        var basket = await repository.GetBasket(username, cancellationToken);
        await cache.SetStringAsync(basket.Username, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);
        await cache.SetStringAsync(basket.Username, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(username, cancellationToken);
        await cache.RemoveAsync(username, cancellationToken);

        return true;
    }
}