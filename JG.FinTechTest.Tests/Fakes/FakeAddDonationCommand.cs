using System.Collections.Generic;
using System.Threading.Tasks;
using JG.FinTechTest.Commands;
using JG.FinTechTest.Models;

namespace JG.FinTechTest.Tests.Fakes
{
    internal class FakeAddDonationCommand : IAddDonationCommand
    {
        public FakeAddDonationCommand()
        {
            Documents = new List<Donation>();
        }

        public Task<int> Execute(Donation donation)
        {
            Documents.Add(donation);

            return Task.FromResult(123);
        }

        internal static List<Donation> Documents { get; private set; }
    }
}