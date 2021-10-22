using FluentValidation;

namespace Product.Application.Features.Commands.AddProduct
{
  public class UpdateProductValidator : AbstractValidator<AddProductCommand>
  {
    public UpdateProductValidator()
    {
      RuleFor(p => p.Sku).NotEmpty().WithMessage("{Sku} is required")
        .NotNull()
        .MaximumLength(50).WithMessage("{Sku} must not exceed 50 characters.");

      RuleFor(p => p.Name).NotEmpty().WithMessage("{Name} is required")
        .NotNull()
        .MaximumLength(50).WithMessage("{Name} must not exceed 50 characters.");
    }
  }
}