using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelGuide.DataTransferObjects.Accomodations.Input;

namespace TravelGuide.DataTransferObjects.Accomodations
{
    public class ScrapingTask
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public HotelSearchRequestDto HotelSearchRequestDto { get; set; }
    }
}