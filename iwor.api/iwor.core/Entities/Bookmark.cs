using System;

namespace iwor.core.Entities
{
    public class Bookmark : BaseEntity
    {
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public Guid AuctionId { get; set; }
        public ApplicationUser User { get; set; }
        public Auction Auction { get; set; }
    }
}