using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace JG.FinTechTest.Tests.Controllers
{
    internal class TestHelper
    {
        public static HttpClient CreateFakeHttpClient()
        {
            var webhostBuilder = new WebHostBuilder().UseStartup<TestStartup>();
            var fakeHttpClient = new TestServer(webhostBuilder).CreateClient();
            return fakeHttpClient;
        }
    }
}