using System;

namespace iwor.api.DTOs
{
    public class NewPriceRaiseDto
    {
        public double StartPrice { get; set; }
        public double EndPrice { get; set; }
        public Guid AuctionId { get; set; }
    }
}