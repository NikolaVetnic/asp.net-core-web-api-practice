namespace Basket.Api.Basket.DeleteBasket;

public class DeleteBasketHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        // todo: delete basket from database and cache       
        // await repository.DeleteBasket(command.Username, cancellationToken);

        return new DeleteBasketResult(true);
    }
}

public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
    }
}