using DFC.Api.Location.Models.AzureSearch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Api.Location.Contracts
{
    public interface ISearchIndexService
    {
        Task<int> BuildIndex(IEnumerable<SearchLocationIndex> searchLocations);
    }
}
