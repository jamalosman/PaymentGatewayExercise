using AutoMapper;
using PaymentGateway.Domain.Models;
using PaymentGateway.Messages.Commands;
using PaymentGateway.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Services.MappingProfiles
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<Payment, PaymentRecordModel>();
            CreateMap<Payment, SubmitPaymentCommand>();
            CreateMap<PaymentRequestModel, Payment>()
                .ForMember(
                    d => d.CardExpiry,
                    cfg => cfg.MapFrom(x => new DateTime(x.CardExpiryYear, x.CardExpiryMonth, 1))
                );
        }
    }
}
