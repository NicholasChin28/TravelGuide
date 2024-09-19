using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TravelGuide.DataTransferObjects.Accomodations.Input
{
    public class HotelSearchRequestDto
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "price_min")]
        public int PriceMin { get; set; }
        [FromQuery(Name = "price_max")]
        public int PriceMax { get; set; }
        [FromQuery(Name = "checkin")]
        public string Checkin { get; set; }
        [FromQuery(Name = "checkout")]
        public string Checkout { get; set; }
    }
}