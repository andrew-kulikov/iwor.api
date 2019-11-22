using System;
using System.ComponentModel.DataAnnotations;

namespace iwor.api.DTOs
{
    public class NewPriceRaiseDto
    {
        [Required] public double StartPrice { get; set; }

        [Required] public double EndPrice { get; set; }
        [Required] public Guid AuctionId { get; set; }
    }
}