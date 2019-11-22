using iwor.core.Entities;

namespace iwor.core.Specifications
{
    public class AuctionSpecification : BaseSpecification<Auction>
    {
        public AuctionSpecification(AuctionStatus? status, string userId, string filter)
        {
            if (status != null) AddCriteria(a => a.Status == status);
            if (userId != null) AddCriteria(a => a.OwnerId == userId);
            if (filter != null)
                AddCriteria(a => a.Name.ToLower().Contains(filter.ToLower()) ||
                                 a.Description.ToLower().Contains(filter.ToLower()));

            ApplyOrderByDescending(a => a.Created);
        }
    }
}