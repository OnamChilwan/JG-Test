using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace JG.FinTechTest.Tests
{
    [TestFixture]
    public class GiftAidCalculatorTests
    {
        [Test]
        public void Given_A_Donation_When_Calculating_Gift_Aid_The_Correct_Amount_Is_Returned()
        {
            const decimal donationAmount = 100M;
            var subject = new GiftAidCalculator();
            var result = subject.Calculate(donationAmount);

            Assert.That(result, Is.EqualTo(25M));
        }
    }

    public class GiftAidCalculator
    {
        public decimal Calculate(decimal donationAmount)
        {
            return 0M;
        }
    }
}