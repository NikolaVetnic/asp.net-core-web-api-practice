using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.Api.Endpoints;

public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteOrderCommand(id));
                var response = result.Adapt<DeleteOrderResponse>();

                return Results.Ok(response);
            })
            .WithName("DeleteOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Order")
            .WithDescription("Delete Order");
    }
}

public record DeleteOrderResponse(bool IsSuccess);