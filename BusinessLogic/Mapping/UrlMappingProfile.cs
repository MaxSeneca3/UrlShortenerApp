using AutoMapper;
using BusinessLogic.Dtos;
using DataAccess.Entities;

namespace BusinessLogic.Mapping;

public class UrlMappingProfile : Profile
{
    public UrlMappingProfile()
    {
        CreateMap<ShortUrl, CreateShortUrlDto>(); // Mapping from ShortUrl to CreateShortUrlDto
        CreateMap<CreateShortUrlDto, ShortUrl>(); // Mapping from CreateShortUrlDto to ShortUrl
    }
}

