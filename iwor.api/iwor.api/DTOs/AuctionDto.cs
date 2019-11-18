using System;

namespace iwor.api.DTOs
{
    public class AuctionDto
    {
        public DateTime StartDate { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public double StartPrice { get; set; }
    }
}