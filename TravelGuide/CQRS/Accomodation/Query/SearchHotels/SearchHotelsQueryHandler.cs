using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TravelGuide.DataTransferObjects.Accomodations.Base;
using TravelGuide.Services.Interfaces;

namespace TravelGuide.CQRS.Accomodation.Query.SearchHotels
{
    public class SearchHotelsQueryHandler : IRequestHandler<SearchHotelsQuery, IEnumerable<BaseHotelDto>>
    {
        private readonly IAirbnbScraperService airbnbScraperService;
        private readonly IRabbitMQService rabbitMQService;

        public SearchHotelsQueryHandler
        (
            IAirbnbScraperService airbnbScraperService,
            IRabbitMQService rabbitMQService
        )
        {
            this.airbnbScraperService = airbnbScraperService;
            this.rabbitMQService = rabbitMQService;
        }

        public async Task<IEnumerable<BaseHotelDto>> Handle(SearchHotelsQuery request, CancellationToken cancellationToken)
        {
            var hotels = await airbnbScraperService.ScrapeHotelAsync(request.SearchCriteria.PriceMin, request.SearchCriteria.PriceMax);

            var scrapingTaskEnqueued = false;

            if (!hotels.Any())
            {
                rabbitMQService.PublishScrapingTask(request.SearchCriteria);
                scrapingTaskEnqueued = true;
            }

            return hotels;
        }
    }
}