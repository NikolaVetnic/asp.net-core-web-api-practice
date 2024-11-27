using Discount.Grpc;

namespace Basket.Api.Basket.StoreBasket;

public class StoreBasketCommandHandler(
    IBasketRepository repository,
    DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        // communicate with grpc Discount service
        await DeductDiscounts(command.Cart, cancellationToken);

        // store basket in database (use Marten upsert - if exists update, if not create) and update cache
        await repository.StoreBasket(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart.Username);
    }

    private async Task DeductDiscounts(ShoppingCart cart, CancellationToken cancellationToken)
    {
        // communicate with Discount.Grpc and calculate discounted prices of products into shopping cart
        foreach (var item in cart.Items)
        {
            var coupon = await discountProto.GetDiscountAsync(
                new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);

            item.Price -= coupon.Amount;
        }
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