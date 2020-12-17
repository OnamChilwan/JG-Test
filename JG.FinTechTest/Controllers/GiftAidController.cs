using System.Collections.Generic;
using System.Linq;
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

    public class GiftAidValidator
    {
        public static List<ApiError> Validate(decimal donation)
        {
            var minimumDonation = new MinimumDonation();
            var maximumDonation = new MaximumDonation();
            var errors = new List<ApiError>();

            if (donation < minimumDonation.Amount)
            {
                errors.Add(new ApiError("InvalidDonationAmount", $"Minimum donation amount is {minimumDonation}"));
            }

            if (donation > maximumDonation.Amount)
            {
                errors.Add(new ApiError("InvalidDonationAmount", $"Maximum donation amount is {maximumDonation}"));
            }

            return errors;
        }
    }
}
