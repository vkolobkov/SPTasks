using ServiceStack;
using ServiceStack.FluentValidation;
using SP_Task1.ServiceModel;
using System;

namespace SP_Task1.ServiceInterface
{
    public class GetInvoiceValidator : AbstractValidator<GetInvoice>
    {
        public GetInvoiceValidator()
        {
            RuleSet(ApplyTo.Get, () => {
                RuleFor(r => r.Id).NotEqual(Guid.Empty).WithMessage("Specify nonempty Id for Invoice.");
            });
        }
    }
}
