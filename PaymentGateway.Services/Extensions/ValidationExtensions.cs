using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Services.Extensions
{
    public static class ValidationExtensions
    {

        public static void ValidateForServices<T>(this AbstractValidator<T> validator, T model)
        {
            validator.Validate(model, options => options
                .IncludeRuleSets("PaymentGateway.Services")
                .ThrowOnFailures());
        }
    }
}
