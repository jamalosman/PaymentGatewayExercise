using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Services.Exceptions;
using PaymentGateway.Services.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Tests.UnitTests.ServiceTests
{
    [TestClass]
    public class PaymentServiceTests
    {

        public PaymentService GetService()
        {
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            var repository = fixture.Freeze<Mock<IPaymentsRepository>>();
            repository
                .Setup(x => x.GetPayment(It.IsAny<int>()))
                .Returns(fixture.Create<Task<Payment>>());
            repository
                .Setup(x => x.GetPayment(It.IsAny<Guid>()))
                .Returns(fixture.Create<Task<Payment>>());

            var sut = fixture.Create<PaymentService>();
            return sut;
        }

        [TestMethod]
        public async Task GetRecordInt_ShouldReturn_FoundRecord()
        {
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            var repository = fixture.Freeze<Mock<IPaymentsRepository>>();
            repository
                .Setup(x => x.GetPayment(It.IsAny<int>()))
                .Returns(fixture.Create<Task<Payment>>());

            var sut = fixture.Create<PaymentService>();
            var paymentRecord = await sut.GetPaymentRecord(fixture.Create<int>());

            Assert.IsNotNull(paymentRecord);
        }

        [TestMethod]
        public async Task GetRecordGuid_ShouldReturn_FoundRecord()
        {
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            var repository = fixture.Freeze<Mock<IPaymentsRepository>>();
            repository
                .Setup(x => x.GetPayment(It.IsAny<Guid>()))
                .Returns(fixture.Create<Task<Payment>>());

            var sut = fixture.Create<PaymentService>();
            var paymentRecord = await sut.GetPaymentRecord(fixture.Create<Guid>());

            Assert.IsNotNull(paymentRecord);
        }

        [TestMethod]
        public void GetRecordInt_ShouldThrow_NotFound()
        {
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            var repository = fixture.Freeze<Mock<IPaymentsRepository>>();
            repository
                .Setup(x => x.GetPayment(It.IsAny<int>()))
                .Returns(async () => null);

            var sut = fixture.Create<PaymentService>();

            Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await sut.GetPaymentRecord(fixture.Create<int>());
            });
        }

        [TestMethod]
        public void GetRecordGuid_ShouldThrow_NotFound()
        {
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            var repository = fixture.Freeze<Mock<IPaymentsRepository>>();
            repository
                .Setup(x => x.GetPayment(It.IsAny<Guid>()))
                .Returns(async () => null);

            var sut = fixture.Create<PaymentService>();

            Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await sut.GetPaymentRecord(fixture.Create<Guid>());
            });
        }
    }
}
