using ServiceStack;
using ServiceStack.FluentValidation;
using SP_Task1.ServiceModel;
using System;

namespace SP_Task1.ServiceInterface
{
    public class GetPurchaseOrderValidator : AbstractValidator<GetPurchaseOrder>
    {
        public GetPurchaseOrderValidator()
        {
            RuleSet(ApplyTo.Get, () => {
                RuleFor(r => r.Id).NotEqual(Guid.Empty).WithMessage("Specify nonempty Id for PurchaseOrder.");
            });
        }
    }
}
