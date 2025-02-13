// ReSharper disable NullableWarningSuppressionIsUsed

namespace Shopping.Web.Pages;

public class ProductDetailsModel(
    ICatalogueService catalogueService,
    IBasketService basketService,
    ILogger<ProductDetailsModel> logger)
    : PageModel
{
    public ProductModel Product { get; set; } = default!;

    [BindProperty] public string Color { get; set; } = default!;

    [BindProperty] public int Quantity { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid productId)
    {
        var response = await catalogueService.GetProduct(productId);
        Product = response.Product;

        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
    {
        logger.LogInformation("Add to cart button clicked");
        var productResponse = await catalogueService.GetProduct(productId);

        var basket = await basketService.LoadUserBasket();

        basket.Items.Add(new ShoppingCartItemModel
        {
            ProductId = productId,
            ProductName = productResponse.Product.Name,
            Price = productResponse.Product.Price,
            Quantity = Quantity,
            Color = Color
        });

        await basketService.StoreBasket(new StoreBasketRequest(basket));

        return RedirectToPage("Cart");
    }
}
