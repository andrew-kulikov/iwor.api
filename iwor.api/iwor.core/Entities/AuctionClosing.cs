using System;

namespace iwor.core.Entities
{
    public class AuctionClosing
    {
        public Guid Id { get; set; }
        public Auction Auction { get; set; }
        public Guid AuctionId { get; set; }
        public ApplicationUser Winner { get; set; }
        public string WinnerId { get; set; }
        public double EndPrice { get; set; }
        public DateTime Closed { get; set; }
    }
}