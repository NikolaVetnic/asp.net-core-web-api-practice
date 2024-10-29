namespace Basket.Api.Exceptions;

public class BasketNotFoundException(string username) : NotFoundException("Basket", username);
