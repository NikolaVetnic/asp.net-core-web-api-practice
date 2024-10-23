namespace Basket.Api.Basket.GetBasket;

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{username}", async (string username, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery(username));
            var response = result.Adapt<GetBasketResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .Produces<GetBasketResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
    }
}

// public record GetBasketRequest(string Username);

public record GetBasketResponse(ShoppingCart Cart);