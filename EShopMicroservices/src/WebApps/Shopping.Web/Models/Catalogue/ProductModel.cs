// ReSharper disable NullableWarningSuppressionIsUsed

namespace Shopping.Web.Models.Catalogue;

public class ProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<string> Category { get; set; } = [];
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;
    public decimal Price { get; set; }
}

// Wrapper classes
public record GetProductsResponse(IEnumerable<ProductModel> Products);

public record GetProductByCategoryResponse(IEnumerable<ProductModel> Products);

public record GetProductByIdResponse(ProductModel Product);
