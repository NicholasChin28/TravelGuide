using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelGuide.DataTransferObjects.Accomodations.Base;

namespace TravelGuide.Services.Interfaces
{
    public interface IAirbnbScraperService
    {
        Task<IEnumerable<BaseHotelDto>> ScrapeHotelAsync(int priceMin, int priceMax);
    }
}