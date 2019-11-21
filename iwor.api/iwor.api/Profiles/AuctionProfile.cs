using System;
using System.Globalization;
using AutoMapper;
using iwor.api.DTOs;
using iwor.core.Entities;

namespace iwor.api.Profiles
{
    public class AuctionProfile : Profile
    {
        public AuctionProfile()
        {
            var df = "yyyy-MM-dd";

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
                .ForMember(dest => dest.EndDate,
                    opt => opt.MapFrom(src => DateTime.ParseExact(src.EndDate, df, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.StartDate,
                    opt => opt.MapFrom(src => DateTime.ParseExact(src.StartDate, df, CultureInfo.InvariantCulture)))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.EndDate,
                    opt => opt.MapFrom(src => src.EndDate.ToString(df, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.StartDate,
                    opt => opt.MapFrom(src => src.StartDate.ToString(df, CultureInfo.InvariantCulture)));

            CreateMap<FavoriteDto, Bookmark>()
                .BeforeMap((s, d) => d.Created = DateTime.Now);

            CreateMap<ApplicationUser, UserProfile>()
                .ForMember(
                    dest => dest.Birthday,
                    opt => opt.MapFrom(
                        src => src.Birthday.HasValue
                            ? src.Birthday.Value.ToString(df, CultureInfo.InvariantCulture)
                            : null))
                .AfterMap((s, d) =>
                {
                    d.PhoneNumber ??= string.Empty;
                    d.Address ??= string.Empty;
                    d.FirstName ??= string.Empty;
                    d.LastName ??= string.Empty;
                })
                .ForMember(dest => dest.RegistrationDate,
                    opt => opt.MapFrom(src => src.RegistrationDate.ToString(df, CultureInfo.InvariantCulture)))
                .ReverseMap()
                .ForMember(dest => dest.Birthday,
                    opt => opt.MapFrom(src => DateTime.ParseExact(src.Birthday, df, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.RegistrationDate,
                    opt => opt.MapFrom(src =>
                        DateTime.ParseExact(src.RegistrationDate, df, CultureInfo.InvariantCulture)));
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.Birthday,
                    opt => opt.MapFrom(src => DateTime.ParseExact(src.Birthday, df, CultureInfo.InvariantCulture)))
                .ReverseMap()
                .ForMember(dest => dest.Birthday,
                    opt => opt.MapFrom(
                        src => src.Birthday.HasValue
                            ? src.Birthday.Value.ToString(df, CultureInfo.InvariantCulture)
                            : null));

            CreateMap<PriceRaiseDto, PriceRaise>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => DateTime.ParseExact(src.Date, df, CultureInfo.InvariantCulture)))
                .ReverseMap()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date.ToString(df, CultureInfo.InvariantCulture)));

            CreateMap<NewPriceRaiseDto, PriceRaise>().ReverseMap();
        }
    }
}