using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;

namespace Explorer.Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<EquipmentDto, Equipment>().ReverseMap();
        CreateMap<MapObjectDto, MapObject>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => MapObjectTypeFromString(src.Category)))
            .ReverseMap();
        CreateMap<TourPreferenceDto, TourPreference>().ReverseMap();
        CreateMap<CheckpointDto, Checkpoint>().ReverseMap();
        CreateMap<TourDto, Tour>().ReverseMap();
        CreateMap<ReportedIssueDto, ReportedIssue>().ReverseMap();
        CreateMap<TourRatingDto, TourRating>().ReverseMap();
        CreateMap<ShoppingCartDto, ShoppingCart>().ReverseMap();
        CreateMap<OrderItemDto, OrderItem>().ReverseMap();
    }

    private MapObjectType MapObjectTypeFromString(string category)
    {
        if (Enum.TryParse<MapObjectType>(category, true, out var mapObjectType))
        {
            return mapObjectType;
        }

        return MapObjectType.Other;
    }
}