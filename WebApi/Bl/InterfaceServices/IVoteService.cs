using Dl.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.InterfaceServices
{
    public interface IVoteService
    {
        Task<Vote> GetVoteAsync(int userId);
        Task AddVoteAsync(Vote voteDto);
        Task<int> GetVotesCountAsync(int imageId);
    }
}
