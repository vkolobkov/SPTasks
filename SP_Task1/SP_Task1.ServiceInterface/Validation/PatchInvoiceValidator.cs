using ServiceStack;
using ServiceStack.FluentValidation;
using SP_Task1.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Task1.ServiceInterface.Validation
{
    public class PatchInvoiceValidator : AbstractValidator<PatchInvoice>
    {
        public PatchInvoiceValidator()
        {
            RuleSet(ApplyTo.Patch, () => {
                RuleFor(r => r.Id).NotEqual(Guid.Empty).WithMessage("Specify nonempty Id for Invoice.");
            });
        }
    }
}
