namespace Basket.Api.Basket.StoreBasket;

public class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        var cart = command.Cart;

        // todo: store basket in database (use Marten upsert - if exists update, if not create)
        // todo: update cache

        return new StoreBasketResult("username");
    }
}

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string Username);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.Username).NotEmpty().WithMessage("Username is required");
    }
}