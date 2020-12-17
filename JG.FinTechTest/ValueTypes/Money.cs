namespace JG.FinTechTest.ValueTypes
{
    public abstract class Money
    {
        public abstract decimal Amount { get; }

        public override string ToString()
        {
            return $"£{Amount:N}";
        }
    }
}