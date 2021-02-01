using PaymentGateway.Messages.Events;
using PaymentGateway.Services.Interfaces;
using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PersistPaymentsWorker.Handlers
{
    public class PaymentCompletedHandler : IHandleMessages<PaymentCompletedEvent>
    {
        private readonly IPaymentService _paymentService;

        public PaymentCompletedHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Handle(PaymentCompletedEvent message)
        {
            await _paymentService.UpdatePayment(message.PaymentId, message.BankPaymentId, message.Success);
        }
    }
}
