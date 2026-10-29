using FluentValidation;

namespace Finovo.Application.Features.Invoices.Commands.AddInvoiceItem;

public sealed class AddInvoiceItemCommandValidator : AbstractValidator<AddInvoiceItemCommand>
{
    public AddInvoiceItemCommandValidator()
    {
        RuleFor(x => x.InvoiceId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Hours).GreaterThan(0);
        RuleFor(x => x.HourlyRateAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
        RuleFor(x => x.TaxRatePercent).Must(x => x is 0 or 10 or 20)
            .WithMessage("Steuersatz muss 0, 10 oder 20 sein.");
    }
}
