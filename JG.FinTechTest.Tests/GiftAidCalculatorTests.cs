using JG.FinTechTest.ValueTypes;
using NUnit.Framework;

namespace JG.FinTechTest.Tests
{
    [TestFixture]
    public class GiftAidCalculatorTests
    {
        [TestCase(100, 25)]
        [TestCase(10.50, 2.625)]
        [TestCase(1234, 308.5)]
        public void Given_A_Donation_When_Calculating_Gift_Aid_Then_Correct_Amount_Is_Returned(decimal donationAmount, decimal expectedGiftAidAmount)
        {
            var subject = new GiftAid(donationAmount);
            Assert.That(subject.Amount, Is.EqualTo(expectedGiftAidAmount));
        }
    }
}