using System.Collections.Generic;
using System.Threading.Tasks;
using iwor.core.Entities;

namespace iwor.core.Services
{
    public interface IAuctionService
    {
        Task<ICollection<Auction>> GetUserActiveAuctions(string userId);
    }
}