using BankServices.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankServices.Interfaces
{
    public interface IAquiringBankPaymentService
    {
        public void CompletePayment(SubmitPaymentCommand command);
    }
}
