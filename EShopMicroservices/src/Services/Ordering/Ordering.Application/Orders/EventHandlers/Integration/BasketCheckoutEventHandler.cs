using BuildingBlocks.Messaging.Events;
using MassTransit;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Domain.Enums;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler(ISender sender, ILogger<BasketCheckoutEventHandler> logger)
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        // ToDo: Create new order and start order fullfillment process
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

        await sender.Send(context.Message.ToCreateOrderCommand());
    }
}

file static class BasketCheckoutEventExtensions
{
    public static CreateOrderCommand ToCreateOrderCommand(this BasketCheckoutEvent message)
    {
        // Create full order with incoming event data
        var addressDto = new AddressDto(
            message.FirstName, message.LastName, message.EmailAddress, message.AddressLine, message.Country,
            message.State, message.ZipCode);

        var paymentDto = new PaymentDto(
            message.CardName, message.CardNumber, message.Expiration, message.Cvv, message.PaymentMethod);

        var orderId = Guid.NewGuid();

        var orderDto = new OrderDto(
            orderId,
            message.CustomerId,
            message.Username[..5], // ToDo: Generate OrderName properly 
            addressDto, addressDto, paymentDto,
            OrderStatus.Pending,
            [
                new OrderItemDto(orderId, new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"), 2, 500),
                new OrderItemDto(orderId, new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"), 1, 400)
            ]);

        return new CreateOrderCommand(orderDto);
    }
}
