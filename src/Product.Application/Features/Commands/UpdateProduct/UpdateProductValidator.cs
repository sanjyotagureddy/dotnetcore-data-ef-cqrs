using FluentValidation;

namespace Product.Application.Features.Commands.UpdateProduct
{
  public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
  {
    public UpdateProductValidator()
    {
      RuleFor(p => p.Name).NotEmpty().WithMessage("{Name} is required")
        .NotNull()
        .MaximumLength(50).WithMessage("{Name} must not exceed 50 characters.");
    }
  }
}