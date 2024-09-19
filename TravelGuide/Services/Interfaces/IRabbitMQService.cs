using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelGuide.DataTransferObjects.Accomodations.Input;

namespace TravelGuide.Services.Interfaces
{
    public interface IRabbitMQService
    {
        void PublishScrapingTask(HotelSearchRequestDto request);
    }
}