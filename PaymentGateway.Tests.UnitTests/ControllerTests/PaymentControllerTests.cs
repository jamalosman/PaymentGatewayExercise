using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Services.Models;
using PaymentGateway.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Tests.UnitTests.ControllerTests
{
    [TestClass]
    public class PaymentControllerTests
    {
        public PaymentsController GetController()
        {
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            fixture.Freeze<Mock<IPaymentService>>();
            var sut = fixture.Build<PaymentsController>()
                .OmitAutoProperties()
                .Create();
            return sut;
        }

        [TestMethod]
        public async Task SubmitPayment_ValidModel_Returns_302ToGetPaymentRecord()
        {
            var sut = GetController();
            var result = await sut.SubmitPayment(new PaymentRequestModel
            {
                Amount = 100,
                Currency = "GBP",
                CardExpiryMonth = 1,
                CardExpiryYear = 1,
                CardFullName = "John Doe",
                CardNumber = "1234567887654321",
                MerchantId = 1,
            });

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirectResult = (result as RedirectToActionResult);

            Assert.AreEqual(nameof(PaymentsController.GetPaymentRecordById),redirectResult.ActionName);
        }




    }
}
