using iwor.core.Entities;

namespace iwor.core.Specifications
{
    public class AuctionSpecification : BaseSpecification<Auction>
    {
        public AuctionSpecification(AuctionStatus? status, string userId)
        {
            if (status != null) AddCriteria(a => a.Status == status);
            if (userId != null) AddCriteria(a => a.OwnerId == userId);

            ApplyOrderByDescending(a => a.Created);
        }
    }
}