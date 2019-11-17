using System;

namespace iwor.DAL.Models
{
    public class Auction
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public ApplicationUser Owner { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public double StartPrice { get; set; }
        public AuctionClosing Closing { get; set; }
        public Guid ClosingId { get; set; }
    }
}