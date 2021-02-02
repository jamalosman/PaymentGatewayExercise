using AutoMapper;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Messages.Commands;
using PaymentGateway.Services.Exceptions;
using PaymentGateway.Services.Extensions;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Services.Models;
using PaymentGateway.Services.Validators;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        private readonly PaymentRequestValidator _validator;
        private readonly ClaimsPrincipal _currentUser;

        public PaymentService(
            IPaymentsRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IBus bus,
            PaymentRequestValidator validator,
            ClaimsPrincipal currentUser)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bus = bus;
            _validator = validator;
            _currentUser = currentUser;
        }
        public async Task<PaymentRecordModel> GetPaymentRecord(int id)
        {
            var payment = await _repository.GetPayment(id);
            if (payment == null) throw new NotFoundException(nameof(Payment), id);
            // if the paynent belongs to another merchant, this merchant shouldn't know it exists
            if (payment.MerchantId != GetCurrentUserId() && GetCurrentUserRole() != "admin") throw new NotFoundException(nameof(Payment), id);

            var paymentRecord = _mapper.Map<PaymentRecordModel>(payment);
            return paymentRecord;
        }

        public async Task<PaymentRecordModel> GetPaymentRecord(Guid bankPaymentId)
        {
            var payment = await _repository.GetPayment(bankPaymentId);
            
            if (payment == null) throw new NotFoundException(nameof(Payment), bankPaymentId);
            // if the paynent belongs to another merchant, this merchant shouldn't know it exists
            if (payment.MerchantId != GetCurrentUserId() && GetCurrentUserRole() != "admin") throw new NotFoundException(nameof(Payment), bankPaymentId);

            var paymentRecord = _mapper.Map<PaymentRecordModel>(payment);
            
            return paymentRecord;
        }

        public async Task<int> SubmitPayment(PaymentRequestModel requestModel)
        {
            if (!_currentUser.IsInRole("merchant"))
            {
                throw new UnauthorizedResourceAccessException("Only merchants can make payments");
            }

            string idString = _currentUser.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            requestModel.MerchantId = GetCurrentUserId() ?? 0;
            
            _validator.ValidateForServices(requestModel);
            
            var payment = _mapper.Map<Payment>(requestModel);
            payment.Submitted = DateTime.Now;
            payment.Status = PaymentStatus.NotSubmitted;
            payment.CardNumber = $"************{payment.CardNumber.Substring(12)}";
            await _repository.AddPayment(payment);
            await _unitOfWork.Commit();

            var command = _mapper.Map<SubmitPaymentCommand>(payment);
            await _bus.Publish(command);

            payment.Status = PaymentStatus.Submitted;
            await _unitOfWork.Commit();

            return payment.Id;
        }

        public async Task UpdatePayment(int id, Guid bankPaymentId, bool success)
        {
            var payment = await _repository.GetPayment(id);
            if (payment == null) throw new NotFoundException(nameof(Payment), id);

            payment.BankPaymentId = bankPaymentId;
            payment.Status = success ? PaymentStatus.Success : PaymentStatus.Failure;
            payment.Processed = DateTime.Now;

            await _repository.UpdatePayment(payment);
            await _unitOfWork.Commit();
        }

        private int? GetCurrentUserId()
        {
            string idString = _currentUser.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if (!int.TryParse(idString, out int id))
            {
                return null;
            }

            return id;
        }

        private string GetCurrentUserRole()
        {
            return _currentUser.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
        }
    }
}
