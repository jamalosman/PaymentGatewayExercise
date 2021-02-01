using BankServices.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentGateway.Messages.Commands;
using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankServices.SubmitPaymentsWorker.Handlers
{
    public class SubmitPaymentHandler : IHandleMessages<SubmitPaymentCommand>
    {
        private readonly IAquiringBankPaymentService _service;

        public SubmitPaymentHandler(IAquiringBankPaymentService service)
        {
            _service = service;
        }

        public async Task Handle(SubmitPaymentCommand message)
        {
            await _service.CompletePayment(message);
        }
    }
}
