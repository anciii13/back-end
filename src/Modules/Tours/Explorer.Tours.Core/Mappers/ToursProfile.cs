using System.Linq;
using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using FluentResults;

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
        CreateMap<ReportedIssueDto, ReportedIssue>().ReverseMap();
        CreateMap<TourRatingDto, TourRating>().ReverseMap();
        CreateMap<TourDto, Tour>().ReverseMap();
        CreateMap<PublishedTourDto, PublishedTour>().ReverseMap();
        CreateMap<ArchivedTourDto, ArchivedTour>().ReverseMap();
        CreateMap<TourTimeDto, TourTime>().ReverseMap();


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