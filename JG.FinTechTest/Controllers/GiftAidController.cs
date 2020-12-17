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
            if (!GiftAidValidator.IsValid(donation))
            {
                return new BadRequestResult();
            }

            var giftAid = new GiftAid(donation);
            var giftAidResponse = new GiftAidResponse { DonationAmount = donation, GiftAidAmount = giftAid.Amount };
            return new OkObjectResult(giftAidResponse);
        }
    }

    public class GiftAidValidator
    {
        public static bool IsValid(decimal donation)
        {
            if (donation < 2m)
            {
                return false;
            }

            if (donation > 100000.00m)
            {
                return false;
            }

            return true;
        }
    }
}
