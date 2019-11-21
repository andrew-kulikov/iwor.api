using System;

namespace iwor.core.Entities
{
    public class PriceRaise : BaseEntity
    {
        public double StartPrice { get; set; }
        public double EndPrice { get; set; }
        public DateTime Date { get; set; }
        public Auction Auction { get; set; }
        public Guid AuctionId { get; set; }

        public string RaisedUserId { get; set; }
        public ApplicationUser RaisedUser { get; set; }
    }
}