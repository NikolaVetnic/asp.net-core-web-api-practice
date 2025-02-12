using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.Api.Basket.CheckoutBasket;

public class CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository
            .GetBasket(command.BasketCheckoutDto.Username, cancellationToken);

        if (basket == null)
            return new CheckoutBasketResult(false);

        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        await publishEndpoint.Publish(eventMessage, cancellationToken);

        await repository.DeleteBasket(command.BasketCheckoutDto.Username, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto)
            .NotNull().WithMessage("BasketCheckoutDto can't be null");

        RuleFor(x => x.BasketCheckoutDto.Username)
            .NotEmpty().WithMessage("Username is required");
    }
}
