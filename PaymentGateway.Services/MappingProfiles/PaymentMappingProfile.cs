using AutoMapper;
using PaymentGateway.Domain.Models;
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
            CreateMap<Payment, PaymentRecordModel>().ReverseMap();
        }
    }
}
