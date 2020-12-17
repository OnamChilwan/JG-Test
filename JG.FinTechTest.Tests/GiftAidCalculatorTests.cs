using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace JG.FinTechTest.Tests
{
    [TestFixture]
    public class GiftAidCalculatorTests
    {
        [TestCase(100, 25)]
        [TestCase(200, 50)]
        public void Given_A_Donation_When_Calculating_Gift_Aid_The_Correct_Amount_Is_Returned(decimal donationAmount, decimal expectedGiftAidAmount)
        {
            var subject = new GiftAidCalculator();
            var result = subject.Calculate(donationAmount);

            Assert.That(result, Is.EqualTo(expectedGiftAidAmount));
        }
    }

    public class GiftAidCalculator
    {
        public decimal Calculate(decimal donationAmount)
        {
            return 25M;
        }
    }
}