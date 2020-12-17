using JG.FinTechTest.Models;
using JG.FinTechTest.ValueTypes;
using Microsoft.AspNetCore.Mvc;

namespace JG.FinTechTest.Controllers
{
    [Route("api/giftaid")]
    [ApiController]
    public class GiftAidController
    {
        [HttpGet("{donation:decimal}")]
        public IActionResult Get([FromRoute] decimal donation)
        {
            if (donation < 2m)
            {
                return new BadRequestResult();
            }

            var giftAid = new GiftAid(donation);
            var giftAidResponse = new GiftAidResponse { DonationAmount = donation, GiftAidAmount = giftAid.Amount };
            return new OkObjectResult(giftAidResponse);
        }
    }
}
