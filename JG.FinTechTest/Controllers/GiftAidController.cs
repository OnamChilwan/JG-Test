using System.Linq;
using JG.FinTechTest.Commands;
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
        private readonly IAddDonationCommand _addDonationCommand;

        public GiftAidController(IAddDonationCommand addDonationCommand)
        {
            _addDonationCommand = addDonationCommand;
        }

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

        [HttpPost]
        public IActionResult Post([FromBody]Donation donation)
        {
            _addDonationCommand.Execute(donation);

            return new CreatedResult(string.Empty, donation);
        }
    }
}
