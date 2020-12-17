using System;
using System.Linq;
using System.Threading.Tasks;
using JG.FinTechTest.Commands;
using JG.FinTechTest.Models;
using JG.FinTechTest.Services;
using JG.FinTechTest.ValueTypes;
using Microsoft.AspNetCore.Mvc;

namespace JG.FinTechTest.Controllers
{
    [Route("api/giftaid")]
    [ApiController]
    public class GiftAidController : ControllerBase
    {
        private readonly IAddDonationCommand _addDonationCommand;

        public GiftAidController(IAddDonationCommand addDonationCommand)
        {
            _addDonationCommand = addDonationCommand ?? throw new ArgumentNullException(nameof(addDonationCommand));
        }

        [HttpGet]
        public IActionResult Get(decimal donation)
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
        public async Task<IActionResult> Post([FromBody]Donation donation)
        {
            var giftAid = new GiftAid(donation.DonationAmount);
            donation.GiftAid = giftAid.Amount;

            var id = await _addDonationCommand.Execute(donation);

            return new CreatedResult($"location/to/get/resource/{id}", donation);
        }
    }
}
