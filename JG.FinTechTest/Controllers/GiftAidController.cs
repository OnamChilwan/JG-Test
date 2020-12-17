using System.Linq;
using JG.FinTechTest.Models;
using JG.FinTechTest.Services;
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
            var errors = GiftAidValidator.Validate(donation);

            if (errors.Any())
            {
                return new BadRequestObjectResult(errors);
            }

            var giftAid = new GiftAid(donation);
            var giftAidResponse = new GiftAidResponse { DonationAmount = donation, GiftAidAmount = giftAid.Amount };
            return new OkObjectResult(giftAidResponse);
        }
    }
}
