using ServiceStack;
using ServiceStack.FluentValidation;
using SP_Task1.ServiceModel;
using System;

namespace SP_Task1.ServiceInterface.Validation
{
    public class CreateInvoiceValidator : AbstractValidator<CreateInvoice>
    {
        private static double tolerance = 0.5;
        public CreateInvoiceValidator()
        {
            RuleSet(ApplyTo.Post, () => {
                RuleFor(r => r.Number).NotEmpty().WithMessage("Specify Number for Invoice.");
                RuleFor(r => r.CreatedDate).NotEmpty().WithMessage("Specify CreatedDate for Invoice.");
                RuleFor(r => r.DueDate).GreaterThan(DateTime.Now.Date).WithMessage("DueDate of Invoice should be greater than today.");
                RuleFor(r => r.VendorId).NotEqual(Guid.Empty).WithMessage("Specify nonempty VendorId for Invoice.");
                RuleFor(r => r.CustomerId).NotEqual(Guid.Empty).WithMessage("Specify nonempty CustomerId for Invoice.");
                RuleFor(r => r.TaxAmount).GreaterThanOrEqualTo(0).WithMessage("TaxAmount of Invoice should be greater than or equal to 0.");
                RuleFor(r => r.NetAmount).GreaterThanOrEqualTo(0).WithMessage("NetAmount of Invoice should be greater than or equal to 0.");
                RuleFor(r => r.TotalPrice).GreaterThanOrEqualTo(0).WithMessage("TotalPrice of Invoice should be greater than or equal to 0.");
                RuleFor(r => r.TotalPrice).Must((invoice, total) =>
                {
                    return Math.Abs((total - invoice.NetAmount - invoice.TaxAmount).Value) <= tolerance;
                }).WithMessage($"TotalPrice should be equals to NetAmount+TaxAmount with tolerance:{tolerance}.");
                RuleFor(r => r.Lines).NotEmpty().WithMessage("Specify at least one Line for Invoice.");
                RuleForEach(r => r.Lines).SetValidator(new InvoiceLineValidator());
            });
        }
    }
}
