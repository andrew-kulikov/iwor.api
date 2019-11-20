﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace iwor.api.DTOs
{
    public class AuctionDto : NewAuctionDto
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; }
        public string Status { get; set; }
        public ICollection<PriceRaiseDto> PriceRaises { get; set; }
        public double CurrentPrice => PriceRaises.Max(r => r.EndPrice);
    }
}