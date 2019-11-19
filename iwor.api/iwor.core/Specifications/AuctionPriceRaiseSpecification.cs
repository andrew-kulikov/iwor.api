using System;
using System.Linq.Expressions;
using iwor.core.Entities;

namespace iwor.core.Specifications
{
    public class AuctionPriceRaiseSpecification : BaseSpecification<PriceRaise>
    {
        public AuctionPriceRaiseSpecification(Expression<Func<PriceRaise, bool>> criteria) : base(criteria)
        {
        }
    }
}