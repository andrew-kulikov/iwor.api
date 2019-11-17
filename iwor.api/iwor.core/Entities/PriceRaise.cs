﻿using System;

namespace iwor.core.Entities
{
    public class PriceRaise
    {
        public Guid Id { get; set; }
        public double StartPrice { get; set; }
        public double EndPrice { get; set; }

        public Auction Auction { get; set; }
        public Guid AuctionId { get; set; }
    }
}