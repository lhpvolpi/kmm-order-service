namespace Kmm.OrderService.Application.Common.Validators;

public static class FluentValidatorExtensions
{
    public static IRuleBuilder<T, Guid> IdValidator<T>(this IRuleBuilder<T, Guid> ruleBuilder, string message = "Id is required.")
        => ruleBuilder.NotEmpty().WithMessage(message);
}
