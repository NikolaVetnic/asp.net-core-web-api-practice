// ReSharper disable NullableWarningSuppressionIsUsed

namespace Shopping.Web.Models.Basket;

public class ShoppingCartModel
{
    public string Username { get; set; } = default!;
    public List<ShoppingCartItemModel> Items { get; set; } = [];
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
}

public class ShoppingCartItemModel
{
    public int Quantity { get; set; } = default!;
    public string Color { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public Guid ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
}

// Wrapper classes
public record GetBasketResponse(ShoppingCartModel Cart);

public record StoreBasketRequest(ShoppingCartModel Cart);

public record StoreBasketResponse(string Username);

public record DeleteBasketResponse(bool IsSuccess);
