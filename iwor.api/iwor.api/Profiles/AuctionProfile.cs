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
                    d.Status = AuctionStatus.Open;
                    d.Created = DateTime.Now;
                })
                .ReverseMap();

            CreateMap<AuctionDto, Auction>()
                .BeforeMap((s, d) =>
                {
                    d.Status = AuctionStatus.Open;
                    d.Created = DateTime.Now;
                })
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<AuctionStatus>(src.Status)))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<FavoriteDto, Bookmark>()
                .BeforeMap((s, d) => d.Created = DateTime.Now);

            CreateMap<ApplicationUser, UserProfile>().AfterMap((s, d) =>
            {
                d.PhoneNumber ??= string.Empty;
                d.Address ??= string.Empty;
                d.FirstName ??= string.Empty;
                d.LastName ??= string.Empty;
            });
            CreateMap<RegisterDto, ApplicationUser>();
            CreateMap<PriceRaiseDto, PriceRaise>().ReverseMap();
            CreateMap<NewPriceRaiseDto, PriceRaise>().ReverseMap();
        }
    }
}