﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Api.Location.Contracts
{
    public interface ILoadLocations
    {
        Task<int> GetLocationsAndUpdateIndexAsync();
    }
}
