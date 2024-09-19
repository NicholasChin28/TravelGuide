using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TravelGuide.DataTransferObjects.Accomodations.Base;
using TravelGuide.DataTransferObjects.Accomodations.Input;

namespace TravelGuide.CQRS.Accomodation.Query.SearchHotels
{
    public record SearchHotelsQuery(HotelSearchRequestDto SearchCriteria) : IRequest<IEnumerable<BaseHotelDto>>;    
}