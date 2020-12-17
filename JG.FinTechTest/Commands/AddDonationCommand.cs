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
            throw new NotImplementedException();
        }
    }
}
