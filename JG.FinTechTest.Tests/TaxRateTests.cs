using JG.FinTechTest.ValueTypes;
using NUnit.Framework;

namespace JG.FinTechTest.Tests
{
    [TestFixture]
    public class TaxRateTests
    {
        [Test]
        public void Given_A_Basic_Tax_Rate_Then_Tax_Rate_Is_Returned_Correctly()
        {
            var subject = new BasicTaxRate();
            Assert.That(subject.Rate, Is.EqualTo(20m));
        }
    }
}