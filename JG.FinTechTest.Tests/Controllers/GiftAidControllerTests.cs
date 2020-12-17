﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using JG.FinTechTest.Commands;
using JG.FinTechTest.Models;
using JG.FinTechTest.ValueTypes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TestStack.BDDfy;

namespace JG.FinTechTest.Tests.Controllers
{
    [TestFixture]
    public class GiftAidControllerTests
    {
        [Test]
        public void SuccessfullyRetrievingGiftAidFromDonation()
        {
            new GiftAidSteps()
                .Given(x => x.GivenADonationOf(12.34m))
                .When(x => x.WhenRequestIsSentToCalculateGiftAid())
                .Then(x => x.ThenAnOkayResponseIsReturned())
                .And(x => x.ThenTheCorrectGiftAidAmountIsReturned())
                .And(x => x.ThenTheCorrectDonationAmountIsReturned())
                .BDDfy();
        }

        [TestCase(1.99)]
        [TestCase(100000.01)]
        public void InvalidDonationAmountsWhenCalculatingGiftAid(decimal donation)
        {
            new GiftAidSteps()
                .Given(x => x.GivenADonationOf(donation))
                .When(x => x.WhenRequestIsSentToCalculateGiftAid())
                .Then(x => x.ThenBadRequestIsReturned())
                .And(x => x.ThenErrorResponseIsReturnedWithErrorCode("InvalidDonationAmount"))
                .BDDfy();
        }

        [Test]
        public void SuccessfullyAddDonation()
        {
            var donation = new Donation { DonationAmount = 10.5m, Name = "Mr Arsene Wenger", PostalCode = "postal code" };
            new GiftAidSteps()
                .Given(x => x.GivenADonationOf(donation))
                .When(x => x.WhenDonationRequestIsMade())
                .Then(x => x.ThenCreatedResultIsReturned())
                .And(x => x.ThenDonationIsReturned(donation))
                .And(x => x.ThenDonationIsPersisted())
                .And(x => x.ThenTheLocationUriIsSet())
                .BDDfy();
        }
    }

    internal class GiftAidSteps
    {
        private readonly HttpClient _httpClient;
        private HttpResponseMessage _httpResponse;
        private GiftAidResponse _giftAidResponse;
        private decimal _donationAmount;
        private Donation _donation;
        private List<ApiError> _errors;

        public GiftAidSteps()
        {
            _httpClient = TestBuilder.CreateFakeHttpClient();
        }

        public void GivenADonationOf(decimal donation)
        {
            _donationAmount = donation;
        }

        public void GivenADonationOf(Donation donation)
        {
            _donation = donation;
        }

        public void WhenRequestIsSentToCalculateGiftAid()
        {
            _httpResponse = _httpClient.GetAsync($"/api/giftaid/{_donationAmount}", CancellationToken.None).GetAwaiter().GetResult();
        }

        public void WhenDonationRequestIsMade()
        {
            _httpResponse = _httpClient
                .PostAsync($"/api/giftaid", _donation, new JsonMediaTypeFormatter())
                .GetAwaiter()
                .GetResult();
        }

        public void ThenTheCorrectGiftAidAmountIsReturned()
        {
            var expectedGiftAid = new GiftAid(_donationAmount);
            Assert.That(_giftAidResponse.GiftAidAmount, Is.EqualTo(expectedGiftAid.Amount));
        }

        public void ThenTheCorrectDonationAmountIsReturned()
        {
            Assert.That(_giftAidResponse.DonationAmount, Is.EqualTo(_donationAmount));
        }

        public void ThenErrorResponseIsReturnedWithErrorCode(string errorCode)
        {
            Assert.That(_errors.Any(x => x.ErrorCode == errorCode));
        }

        public void ThenDonationIsReturned(Donation donation)
        {
            Assert.That(_donation.DonationAmount, Is.EqualTo(donation.DonationAmount));
            Assert.That(_donation.Name, Is.EqualTo(donation.Name));
            Assert.That(_donation.PostalCode, Is.EqualTo(donation.PostalCode));
        }

        public void ThenDonationIsPersisted()
        {
            Assert.That(FakeAddDonationCommand.Documents, Is.Not.Empty);
        }

        public void ThenTheLocationUriIsSet()
        {
            var locationUri = _httpResponse.Headers.Location.ToString();
            Assert.That(locationUri, Is.EqualTo("location/to/get/resource/123"));
        }

        public void ThenAnOkayResponseIsReturned()
        {
            Assert.That(_httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            _giftAidResponse = _httpResponse.Content.ReadAsAsync<GiftAidResponse>().Result;
        }

        public void ThenBadRequestIsReturned()
        {
            Assert.That(_httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            _errors = _httpResponse.Content.ReadAsAsync<List<ApiError>>().Result;
        }

        public void ThenCreatedResultIsReturned()
        {
            Assert.That(_httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            _donation = _httpResponse.Content.ReadAsAsync<Donation>().Result;
        }
    }

    internal class TestBuilder
    {
        public static HttpClient CreateFakeHttpClient()
        {
            var webhostBuilder = new WebHostBuilder().UseStartup<TestStartup>();
            var fakeHttpClient = new TestServer(webhostBuilder).CreateClient();
            return fakeHttpClient;
        }
    }

    public class TestStartup : Startup
    {
        protected override void ConfigureDependencies(IServiceCollection services)
        {
            services.AddSingleton<IAddDonationCommand, FakeAddDonationCommand>();
        }
    }

    internal class FakeAddDonationCommand : IAddDonationCommand
    {
        public FakeAddDonationCommand()
        {
            Documents = new List<Donation>();
        }

        public Task<int> Execute(Donation donation)
        {
            Documents.Add(donation);

            return Task.FromResult(123);
        }

        internal static List<Donation> Documents { get; private set; }
    }
}
