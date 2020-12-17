using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using NUnit.Framework.Constraints;
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
    }

    internal class GiftAidSteps
    {
        private readonly HttpClient _httpClient;
        private HttpResponseMessage _httpResponse;
        private decimal _donation;
        private GiftAidResponse _giftAidResponse;

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
            _httpResponse = _httpClient.GetAsync($"/api/giftaid/", CancellationToken.None).GetAwaiter().GetResult();
        }

        public void ThenAnOkayResponseIsReturned()
        {
            Assert.That(_httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            _giftAidResponse = _httpResponse.Content.ReadAsAsync<GiftAidResponse>().Result;
        }

        public void ThenTheCorrectGiftAidAmountIsReturned()
        {
            Assert.That(_giftAidResponse.GiftAidAmount, Is.GreaterThan(0));
        }

        public void ThenTheCorrectDonationAmountIsReturned()
        {
            Assert.That(_giftAidResponse.DonationAmount, Is.EqualTo(_donation));
        }
    }

    public class GiftAidResponse
    {
        public decimal DonationAmount { get; set; }

        public decimal GiftAidAmount { get; set; }
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
