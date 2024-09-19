using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TravelGuide.CQRS.Accomodation.Query.SearchHotels;
using TravelGuide.DataTransferObjects.Accomodations.Base;
using TravelGuide.DataTransferObjects.Accomodations.Input;

namespace TravelGuide.Controllers
{
    [Route("[controller]")]
    public class AccomodationController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<AccomodationController> _logger;

        public AccomodationController
        (
            IMediator mediator,
            ILogger<AccomodationController> logger
        )
        {
            this.mediator = mediator;
            _logger = logger;
        }

        // public IActionResult Index()
        // {
        //     return Ok();
        // }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return Ok();
        // }

        [HttpGet("search-hotels")]
        public async Task<ActionResult<IEnumerable<BaseHotelDto>>> SearchHotels([FromQuery] HotelSearchRequestDto request)
        {
            // Handler will probably do the following
            // Query the database and return dto for controller to return
            // Save audit log entity record in database
            // Run background task (eg. redis/hangfire/rabbitmq)
            
            var query = new SearchHotelsQuery(request);
            await mediator.Send(query);
            return Ok();
        }
    }
}