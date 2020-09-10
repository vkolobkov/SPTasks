﻿using ServiceStack;
using ServiceStack.FluentValidation;
using SP_Task1.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Task1.ServiceInterface.Validation
{
    public class CreateBusinessPartnerValidator : AbstractValidator<CreateBusinessPartner>
    {
        public CreateBusinessPartnerValidator()
        {
            RuleSet(ApplyTo.Post, () => {
                RuleFor(r => r.Name).NotEmpty().WithMessage("Specify Name for BusinessPartner.");
                RuleFor(r => r.Email).NotEmpty().WithMessage("Specify Email for BusinessPartner.");
                RuleFor(r => r.Telephone).NotEmpty().WithMessage("Specify Telephone for BusinessPartner.");
                RuleFor(r => r.Address).SetValidator(new AddressValidator());
            });
        }
    }
}
