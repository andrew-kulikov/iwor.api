using System;

namespace iwor.core.Entities
{
    public class Auction : BaseEntity
    {
        public DateTime Created { get; set; }
        public DateTime StartDate { get; set; }
        public AuctionStatus Status { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }
        public string Description { get; set; }
        public double StartPrice { get; set; }
    }
}