using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iwor.core.Entities;
using iwor.core.Repositories;
using iwor.core.Specifications;
using MoreLinq;

namespace iwor.core.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IRepository<PriceRaise> _raiseRepository;

        public AuctionService(IRepository<PriceRaise> raiseRepository)
        {
            _raiseRepository = raiseRepository;
        }

        public async Task<ICollection<Auction>> GetUserActiveAuctions(string userId)
        {
            var spec = new AuctionPriceRaiseSpecification(userId);
            var raises = await _raiseRepository.ListAsync(spec);

            var activeAuctions = raises
                .DistinctBy(r => r.AuctionId)
                .Select(r => r.Auction)
                .Where(a => a.Status == AuctionStatus.Open)
                .ToList();

            return activeAuctions;
        }

        public async Task<ICollection<Auction>> GetUserOwnedAuctions(string userId)
        {
            var spec = new AuctionPriceRaiseSpecification();
            var raises = await _raiseRepository.ListAsync(spec);

            var activeAuctions = raises
                .Where(r => r.Auction.Status == AuctionStatus.Closed)
                .GroupBy(r => r.AuctionId)
                .Where(g => g.OrderByDescending(r => r.Date).FirstOrDefault()?.RaisedUserId == userId)
                .SelectMany(g => g.Select(r => r.Auction))
                .DistinctBy(a => a.Id)
                .ToList();

            return activeAuctions;
        }
    }
}