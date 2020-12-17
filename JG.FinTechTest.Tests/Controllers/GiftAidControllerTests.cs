using System.Net;
using System.Net.Http;
using System.Threading;
using JG.FinTechTest.Models;
using JG.FinTechTest.ValueTypes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
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
                .BDDfy();
        }
    }

    internal class GiftAidSteps
    {
        private readonly HttpClient _httpClient;
        private HttpResponseMessage _httpResponse;
        private GiftAidResponse _giftAidResponse;
        private decimal _donation;

        public GiftAidSteps()
        {
            _httpClient = TestBuilder.CreateFakeHttpClient();
        }

        public void GivenADonationOf(decimal donation)
        {
            _donation = donation;
        }

        public void WhenRequestIsSentToCalculateGiftAid()
        {
            _httpResponse = _httpClient.GetAsync($"/api/giftaid/{_donation}", CancellationToken.None).GetAwaiter().GetResult();
        }

        public void ThenAnOkayResponseIsReturned()
        {
            Assert.That(_httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            _giftAidResponse = _httpResponse.Content.ReadAsAsync<GiftAidResponse>().Result;
        }

        public void ThenTheCorrectGiftAidAmountIsReturned()
        {
            var expectedGiftAid = new GiftAid(_donation);
            Assert.That(_giftAidResponse.GiftAidAmount, Is.EqualTo(expectedGiftAid.Amount));
        }

        public void ThenTheCorrectDonationAmountIsReturned()
        {
            Assert.That(_giftAidResponse.DonationAmount, Is.EqualTo(_donation));
        }

        public void ThenBadRequestIsReturned()
        {
            Assert.That(_httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }

    internal class TestBuilder
    {
        public static HttpClient CreateFakeHttpClient()
        {
            var webhostBuilder = new WebHostBuilder().UseStartup<Startup>();
            var fakeHttpClient = new TestServer(webhostBuilder).CreateClient();
            return fakeHttpClient;
        }
    }
}
