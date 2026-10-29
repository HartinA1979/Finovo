using FluentValidation;

namespace Finovo.Application.Features.Projects.Commands.UpdateServiceItem;

public sealed class UpdateServiceItemCommandValidator : AbstractValidator<UpdateServiceItemCommand>
{
    private static readonly int[] ValidTaxRates = [0, 10, 20];

    public UpdateServiceItemCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.ServiceItemId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Hours).GreaterThan(0).LessThanOrEqualTo(9999.99m);
        RuleFor(x => x.HourlyRateAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
        RuleFor(x => x.TaxRatePercent).Must(v => ValidTaxRates.Contains(v))
            .WithMessage("Steuersatz muss 0, 10 oder 20 sein.");
    }
}
