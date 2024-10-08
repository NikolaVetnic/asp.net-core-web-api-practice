namespace Catalogue.Api.Products.GetProducts;

internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : 
    IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsHandler.Handle called with @{Query}", query);

        var products = await session.Query<Product>().ToListAsync(cancellationToken);

        return new GetProductsResult(products);
    }
}

public record GetProductsQuery : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);