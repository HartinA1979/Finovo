using FluentValidation;

namespace Finovo.Application.Features.Invoices.Commands.CreateInvoice;

public sealed class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.IssueDate).NotEmpty();
        RuleFor(x => x.DueDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.IssueDate)
            .WithMessage("Fälligkeitsdatum muss auf oder nach dem Ausstellungsdatum liegen.");
        RuleFor(x => x.Notes).MaximumLength(2000);
    }
}
