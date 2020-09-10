using ServiceStack.FluentValidation;
using SP_Task1.ServiceModel;

namespace SP_Task1.ServiceInterface.Validation
{
    public class PurchaseOrderLineValidator : AbstractValidator<PurchaseOrderLine>
    {
        public PurchaseOrderLineValidator()
        {
            RuleFor(r => r.Description).NotEmpty().WithMessage("Each line in PurchaseOrder should have Description.");
            RuleFor(r => r.Quantity).GreaterThanOrEqualTo(0).WithMessage("PurchaseOrderLine quantity should be positive or zero.");
            RuleFor(r => r.IsPaired).Equal(false).WithMessage("Initial IsPaired state should be false.");
        }
    }
}
