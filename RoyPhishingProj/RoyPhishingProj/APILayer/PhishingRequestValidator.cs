using FluentValidation;
using RoyPhishingProj.BusinessLogicLayer.request;

public class PhishingRequestValidator : AbstractValidator<PhishingRequest>
{
    public PhishingRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name can't exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
