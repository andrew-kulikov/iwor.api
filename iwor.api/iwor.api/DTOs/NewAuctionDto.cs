using System.Collections.Generic;

namespace iwor.api.DTOs
{
    public class NewAuctionDto
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double StartPrice { get; set; }
        public ICollection<string> Images { get; set; }
    }
}