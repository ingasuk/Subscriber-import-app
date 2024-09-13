using FluentValidation;

namespace Subscribers.Models.Subscribers;
public class SubscriberModel
{
    public string Email { get; set; }
    public DateTime ExpirationDate { get; set; }
}

public class SubscriberModelValidator : AbstractValidator<SubscriberModel>
{
    public SubscriberModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required")
                             .EmailAddress();
        RuleFor(x => x.ExpirationDate).NotEmpty().WithMessage("Expiration date is required")
                                      .Must(x => x >= DateTime.UtcNow.Date).WithMessage("Expiration date must be grater then or equal to the current date");
    }
}
