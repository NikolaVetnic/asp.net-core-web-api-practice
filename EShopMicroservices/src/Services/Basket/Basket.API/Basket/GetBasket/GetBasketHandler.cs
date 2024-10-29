using Basket.Api.Data;

namespace Basket.Api.Basket.GetBasket;

public class GetBasketHandler(IBasketRepository repository) 
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(query.Username, cancellationToken);

        return new GetBasketResult(basket);
    }
}

public record GetBasketQuery(string Username) : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCart Cart);