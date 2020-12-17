using NUnit.Framework;

namespace JG.FinTechTest.Tests
{
    [TestFixture]
    public class GiftAidCalculatorTests
    {
        [TestCase(100, 25)]
        [TestCase(10.50, 2.625)]
        [TestCase(1234, 308.5)]
        public void Given_A_Donation_When_Calculating_Gift_Aid_The_Correct_Amount_Is_Returned(decimal donationAmount, decimal expectedGiftAidAmount)
        {
            var subject = new GiftAid(donationAmount);
            Assert.That(subject.Amount, Is.EqualTo(expectedGiftAidAmount));
        }
    }

    public class GiftAid
    {
        private readonly decimal _donation;

        public GiftAid(decimal donation)
        {
            _donation = donation;
        }

        private decimal Calculate()
        {
            var tax = new BasicTaxRate();
            var giftAidRatio = tax.Rate / (100 - tax.Rate);
            return _donation * giftAidRatio;
        }

        public decimal Amount => Calculate();
    }
    

    public class BasicTaxRate
    {
        public decimal Rate => 20m;
    }
}