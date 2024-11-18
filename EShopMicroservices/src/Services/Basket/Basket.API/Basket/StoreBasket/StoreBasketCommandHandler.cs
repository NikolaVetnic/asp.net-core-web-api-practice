namespace Basket.Api.Basket.StoreBasket;

public class StoreBasketCommandHandler(IBasketRepository repository) 
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        // todo: communicate with grpc Discount service
        
        
        // store basket in database (use Marten upsert - if exists update, if not create) and update cache
        await repository.StoreBasket(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart.Username);
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