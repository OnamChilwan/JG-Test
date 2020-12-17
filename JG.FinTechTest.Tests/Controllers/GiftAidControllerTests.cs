using System.Net;
using System.Net.Http;
using System.Threading;
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
                .Then(x => x.ThenAnOkayResponseIsReturned());
        }
    }

    internal class GiftAidSteps
    {
        private readonly HttpClient _httpClient;
        private HttpResponseMessage _httpResponse;
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
