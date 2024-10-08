namespace Catalogue.Api.Products.GetProductById;

internal class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger) : 
    IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIdHandler.Handle called with {@Query}", query);

        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(query.Id);

        return new GetProductByIdResult(product);
    }
}

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);