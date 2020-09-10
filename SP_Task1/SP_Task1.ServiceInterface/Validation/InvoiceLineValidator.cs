using ServiceStack.FluentValidation;
using SP_Task1.ServiceModel;
using System;

namespace SP_Task1.ServiceInterface.Validation
{
    public class InvoiceLineValidator : AbstractValidator<InvoiceLine>
    {
        private static double tolerance = 0.5;
        public InvoiceLineValidator()
        {
            RuleFor(r => r.Description).NotEmpty().WithMessage("Each line in Invoice should have Description.");
            RuleFor(r => r.Quantity).GreaterThanOrEqualTo(0).WithMessage("Invoice quantity should be positive or zero.");
            RuleFor(r => r.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Invoice UnitPrice should be positive or zero.");
            RuleFor(r => r.TaxRate).GreaterThanOrEqualTo(0).WithMessage("Invoice TaxRate should be positive or zero.");
            RuleFor(r => r.TaxAmount).GreaterThanOrEqualTo(0).WithMessage("Invoice TaxAmount should be positive or zero.");
            RuleFor(r => r.TotalPrice).GreaterThanOrEqualTo(0).WithMessage("Invoice TotalPrice should be positive or zero.");
            RuleFor(r => r.PoNumber).NotEmpty().WithMessage("PurchaseOrder number should be specified for each line in Invoice.");
            RuleFor(r => r.IsPaired).Equal(false).WithMessage("Initial IsPaired state should be false.");
            RuleFor(r => r.TaxRate).Must((line, rate) =>
            {
                return Math.Abs((line.UnitPrice * line.Quantity / 100 * rate - line.TaxAmount).Value) <= tolerance;
            }).WithMessage($"TaxAmount doesn't corresponds to TaxRate with tolerance:{tolerance}.");
            RuleFor(r => r.TotalPrice).Must((line, total) =>
            {
                return Math.Abs((total - line.UnitPrice * line.Quantity - line.TaxAmount).Value) <= tolerance;
            }).WithMessage($"Total of line should be equal to UnitPrice*Quantity+TaxAmount with tolerance:{tolerance}.");
        }
    }
}
