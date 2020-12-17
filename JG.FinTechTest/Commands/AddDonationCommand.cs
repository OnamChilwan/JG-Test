using System;
using System.Threading.Tasks;
using JG.FinTechTest.Models;

namespace JG.FinTechTest.Commands
{
    public interface IAddDonationCommand
    {
        Task<int> Execute(Donation donation);
    }

    public class AddDonationCommand : IAddDonationCommand
    {
        public Task<int> Execute(Donation donation)
        {
            /*
             * This I would typically leave throwing an exception but for a working demo perspective I have given it a dummy
             * implementation. I would write an out of process test that would exercise external dependencies and compose an integration that would
             * cover this class and its interaction with a DB.
             */

            return Task.FromResult(DateTime.Now.Millisecond);
        }
    }
}
