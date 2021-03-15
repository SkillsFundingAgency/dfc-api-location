using AutoMapper;
using DFC.Api.Location.Models.AzureSearch;
using DFC.Api.Location.Models.NationalStatisticsLocationApiResponses;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.Location.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class LocationModelProfile : Profile
    {
        public LocationModelProfile()
        {
            CreateMap<LocationResponse, SearchLocationIndex>()
                .ForMember(d => d.LocationId, s => s.MapFrom(s => s.Id));
        }
    }
}
