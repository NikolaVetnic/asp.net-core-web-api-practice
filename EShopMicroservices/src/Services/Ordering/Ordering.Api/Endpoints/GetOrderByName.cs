using Ordering.Application.Orders.Queries.GetOrderByName;

namespace Ordering.Api.Endpoints;

public class GetOrderByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
            {
                var result = await sender.Send(new GetOrderByNameQuery(orderName));
                var response = result.Adapt<GetOrderByNameResponse>();

                return Results.Ok(response);
            })
            .WithName("GetOrderByName")
            .Produces<GetOrderByNameResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Order by Name")
            .WithDescription("Get Order by Name");
    }
}

public record GetOrderByNameResponse(IEnumerable<OrderDto> Orders);