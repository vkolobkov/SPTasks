using ServiceStack;
using ServiceStack.FluentValidation;
using SP_Task1.ServiceModel;
using System;

namespace SP_Task1.ServiceInterface.Validation
{
    public class CreatePurchaseOrderValidator : AbstractValidator<CreatePurchaseOrder>
    {
        public CreatePurchaseOrderValidator()
        {
            RuleSet(ApplyTo.Post, () => {
                RuleFor(r => r.Number).NotEmpty().WithMessage("Specify Number for PurchaseOrder.");
                RuleFor(r => r.CustomerId).NotEqual(Guid.Empty).WithMessage("Specify nonempty CustomerId for PurchaseOrder.");
                RuleFor(r => r.Lines).NotEmpty().WithMessage("Specify at least one Line for PurchaseOrder.");
                RuleForEach(r => r.Lines).SetValidator(new PurchaseOrderLineValidator());
            });
        }
    }
}
