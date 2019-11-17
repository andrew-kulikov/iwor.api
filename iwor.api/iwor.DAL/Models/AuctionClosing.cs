using System;

namespace iwor.DAL.Models
{
    public class AuctionClosing
    {
        public Guid Id { get; set; }
        public Auction Auction { get; set; }
        public Guid AuctionId { get; set; }
        public ApplicationUser Winner { get; set; }
        public string WinnerId { get; set; }
        public decimal EndPrice { get; set; }
        public DateTime Closed { get; set; }
    }
}