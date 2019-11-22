using System;
using System.Linq.Expressions;
using iwor.core.Entities;

namespace iwor.core.Specifications
{
    public class AuctionPriceRaiseSpecification : BaseSpecification<PriceRaise>
    {
        public AuctionPriceRaiseSpecification(string raiserId = null, Guid? auctionId = null)
        {
            AddInclude(r => r.Auction);

            if (auctionId != null) AddCriteria(r => r.AuctionId == auctionId);
            if (raiserId != null) AddCriteria(r => r.RaisedUserId == raiserId);
        }
    }
}