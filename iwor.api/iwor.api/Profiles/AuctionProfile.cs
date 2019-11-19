using System;
using AutoMapper;
using iwor.api.DTOs;
using iwor.core.Entities;

namespace iwor.api.Profiles
{
    public class AuctionProfile : Profile
    {
        public AuctionProfile()
        {
            CreateMap<NewAuctionDto, Auction>()
                .BeforeMap((s, d) =>
                {
                    d.Status = AuctionStatus.Created;
                    d.Created = DateTime.Now;
                })
                .ReverseMap();

            CreateMap<AuctionDto, Auction>()
                .BeforeMap((s, d) =>
                {
                    d.Status = AuctionStatus.Created;
                    d.Created = DateTime.Now;
                })
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<FavoriteDto, Bookmark>()
                .BeforeMap((s, d) => d.Created = DateTime.Now);

            CreateMap<ApplicationUser, UserProfile>();
            CreateMap<RegisterDto, ApplicationUser>();
            CreateMap<PriceRaiseDto, PriceRaise>().ReverseMap();
            CreateMap<NewPriceRaiseDto, PriceRaise>().ReverseMap();
        }
    }
}