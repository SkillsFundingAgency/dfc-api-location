﻿using DFC.Api.Location.Models.AzureSearch;
using DFC.Api.Location.Models.NationalStatisticsLocationApiResponses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Api.Location.Contracts
{
    public interface ILocationsService
    {
        Task<IEnumerable<LocationResponse?>> GetCleanLocationsAsync();
    }
}
