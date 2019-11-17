using System;

namespace iwor.DAL.Models
{
    public class AuctionPhoto
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }
    }
}