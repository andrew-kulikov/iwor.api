using System;

namespace iwor.DAL.Models
{
    public class Bookmark
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public Guid AuctionId { get; set; }
        public ApplicationUser User { get; set; }
        public Auction Auction { get; set; }
    }
}