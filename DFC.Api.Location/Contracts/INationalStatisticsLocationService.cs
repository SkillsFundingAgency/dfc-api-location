using DFC.Api.Location.Models.NationalStatisticsLocationApiResponses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Api.Location.Contracts
{
    public interface INationalStatisticsLocationService
    {
        Task<IEnumerable<LocationsResponse>> GetLocationsAsync();
    }
}
