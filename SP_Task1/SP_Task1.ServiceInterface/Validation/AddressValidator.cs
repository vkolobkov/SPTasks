using ServiceStack.FluentValidation;
using SP_Task1.ServiceModel;

namespace SP_Task1.ServiceInterface.Validation
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(r => r.Street1).NotEmpty().WithMessage("Specify Street1 of Address.");
            RuleFor(r => r.City).NotEmpty().WithMessage("Specify City of Address.");
            RuleFor(r => r.ZipCode).NotEmpty().WithMessage("Specify ZipCode of Address.");
            RuleFor(r => r.Country).NotEmpty().WithMessage("Specify Country of Address.");
        }
    }
}
