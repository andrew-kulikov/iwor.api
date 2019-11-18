﻿using System;
using AutoMapper;
using iwor.api.DTOs;
using iwor.core.Entities;

namespace iwor.api.Profiles
{
    public class AuctionProfile : Profile
    {
        public AuctionProfile()
        {
            CreateMap<AuctionDto, Auction>()
                .BeforeMap((s, d) =>
                {
                    d.Status = AuctionStatus.Created;
                    d.Created = DateTime.Now;
                })
                .ReverseMap();

            CreateMap<BookmarkDto, Bookmark>()
                .BeforeMap((s, d) => d.Created = DateTime.Now);
        }
    }
}