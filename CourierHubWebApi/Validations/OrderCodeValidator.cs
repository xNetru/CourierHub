namespace FluentValidation
{
    public class OrderCodeValidator: AbstractValidator<string>
    {
        public OrderCodeValidator()
        {
            RuleFor(x => x).MaximumLength(50);
        }
    }
}
