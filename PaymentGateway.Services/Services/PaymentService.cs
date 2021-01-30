using AutoMapper;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Services.Exceptions;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentsRepository _repository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaymentRecordModel> GetPaymentRecord(int id)
        {
            var payment = await _repository.GetPayment(id);
            if (payment == null) throw new NotFoundException(nameof(Payment), id);

            var paymentRecord = _mapper.Map<PaymentRecordModel>(payment);
            return paymentRecord;
        }

        public async Task<PaymentRecordModel> GetPaymentRecord(Guid bankPaymentId)
        {
            var payment = await _repository.GetPayment(bankPaymentId);
            if (payment == null) throw new NotFoundException(nameof(Payment), bankPaymentId);

            var paymentRecord = _mapper.Map<PaymentRecordModel>(payment);
            return paymentRecord;
        }

        public async Task<int> SubmitPayment(PaymentRequestModel requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
