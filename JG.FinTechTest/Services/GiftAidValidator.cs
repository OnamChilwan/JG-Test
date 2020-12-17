using System.Collections.Generic;
using JG.FinTechTest.Models;
using JG.FinTechTest.ValueTypes;

namespace JG.FinTechTest.Services
{
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