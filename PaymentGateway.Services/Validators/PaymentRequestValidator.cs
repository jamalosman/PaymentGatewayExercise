using FluentValidation;
using PaymentGateway.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Validators
{
    public class PaymentRequestValidator : AbstractValidator<PaymentRequestModel>
    {

        public PaymentRequestValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Currency).NotNull().Length(3);
            RuleFor(x => x.CardNumber).NotNull().Length(16);
            RuleFor(x => x.CardFullName).NotEmpty();
        }

    }
}
