namespace JG.FinTechTest.ValueTypes
{
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
}