using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaxiBookingAPI.Interfaces;
using TaxiBookingAPI.Model;
using TaxiBookingAPI.TaxiBookingModule;

namespace TaxiBookingAPI.Controllers
{
    /// <summary>
    /// Taxi Booking Controller , Which exposes three Rest End points
    /// POST api/book
    /// POST api/tick
    /// PUT api/reset
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    public class TaxiBookingController : Controller
    {
        private readonly ILogger<TaxiBookingController> _logger;
        private readonly ITaxiBookingService _taxiBookingService;

        /// <summary>
        /// Taxi Booking Controller Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="taxiBookingService"></param>
        public TaxiBookingController(ILogger<TaxiBookingController> logger , ITaxiBookingService taxiBookingService)
        {
            _logger = logger;
            _taxiBookingService = taxiBookingService;
        }
        
        /// <summary>
        /// POST /api/book - returns the nearest available car to the customer location
        /// and returns the total time taken to travel
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/book
        ///     {
        ///         "source": {
        ///             "x": x1,
        ///             "y": y1
        ///            },
        ///        "destination": {
        ///             "x": x2,
        ///             "y": y2
        ///            }
        ///      }
        ///
        /// Sample response:    
        ///
        ///         {
        ///             "car_id": id,
        ///             "total_time": t
        ///         }
        /// </remarks>
        ///
        /// <returns>A newly created resource</returns>
        /// <response code="201">Returns the car_id with total_time</response>
        /// <response code="400">If the item is null</response>    
        /// <param name="location"></param>
        /// <returns></returns>
        [Route("api/book")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostBookTaxi([FromBody] Location  location)
        { 
            // Validation 
            // 400 Bad Request if the Request Payload is not valid
            

            var (carId , totalTime) = _taxiBookingService.BookTaxi(location);
            if (carId == -1 || totalTime == -1) return Ok();
            var result = new { car_id = carId, total_time = totalTime };
            /*return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status201Created
            };*/
            return StatusCode(201, result);
        }
        
        /// <summary>
        /// /api/tick REST endpoint,  advances your service time stamp by 1 time unit
        /// </summary>
        /// <returns></returns>
        [Route("api/tick")]
        [HttpPost]
        public HttpResponseMessage PostTick()
        { 
            _taxiBookingService.IncrementServiceTimeStamp();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        
        /// <summary>
        ///  /api/reset REST endpoint, will reset all cars data back to the initial state
        /// </summary>
        /// <returns></returns>
        [Route("api/reset")]
        [HttpPut]
        public HttpResponseMessage Reset()
        { 
            _taxiBookingService.ResetTaxiBookingSystem();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}