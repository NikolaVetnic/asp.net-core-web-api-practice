namespace Basket.Api.Basket.GetBasket;

public class GetBasketHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        // todo: get basket from database
        // var basket = await _repository.GetBasket(request.Username);

        return new GetBasketResult(new ShoppingCart("username"));
    }
}

public record GetBasketQuery(string Username) : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCart Cart);