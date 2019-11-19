using System.Collections.Generic;
using iwor.core.Entities;

namespace iwor.api.DTOs
{
    public class AuctionDto : NewAuctionDto
    {
        public string OwnerId { get; set; }
        public string Status { get; set; }
        public ICollection<PriceRaise> PriceRaises { get; set; }
    }
}