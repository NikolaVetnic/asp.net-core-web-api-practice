namespace Catalogue.Api.Exceptions;

public class ProductNotFoundException(Guid id) : NotFoundException("Product", id);
