using System;

namespace iwor.core.Entities
{
    public class AuctionPhoto : BaseEntity
    {
        public string Url { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }
    }
}