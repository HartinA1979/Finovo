using FluentValidation;

namespace Finovo.Application.Features.Customers.Commands.UpdateCustomer;

public sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CompanyName).MaximumLength(200).When(x => x.CompanyName is not null);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(256).When(x => x.Email is not null);
        RuleFor(x => x.Phone).MaximumLength(50).When(x => x.Phone is not null);
        RuleFor(x => x.VatNumber).MaximumLength(50).When(x => x.VatNumber is not null);
        RuleFor(x => x.Notes).MaximumLength(2000).When(x => x.Notes is not null);

        When(x => x.Address is not null, () =>
        {
            RuleFor(x => x.Address!.Street).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Address!.City).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address!.ZipCode).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Address!.Country).NotEmpty().MaximumLength(100);
        });
    }
}
