using System;
using System.Collections.Generic;

namespace iwor.api.DTOs
{
    public class NewAuctionDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double StartPrice { get; set; }
        public ICollection<string> Images { get; set; }
    }
}