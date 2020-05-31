using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using TaxiBookingAPI.Controllers;
using TaxiBookingAPI.Interfaces;
using TaxiBookingAPI.Model;
using Xunit;


namespace TaxiBookingAPI.Tests
{
    /// <summary>
    ///  Unit tests for the Taxi Booking Controller Tests
    /// </summary>
    public class TaxiBookingControllerTests
    {
        /// <summary>
        /// 
        /// </summary>
        private TaxiBookingController _controller;
        
        /// <summary>
        /// Mock Taxi Booking Service
        /// </summary>
        private readonly Mock<ITaxiBookingService> _taxiBookingServiceMock = new Mock<ITaxiBookingService>();
        
        /// <summary>
        /// Mock Logger Object
        /// </summary>
        private readonly Mock<ILogger<TaxiBookingController>> _loggerMock = new Mock<ILogger<TaxiBookingController>>();

        /// <summary>
        /// Controller Object
        /// </summary>
        private TaxiBookingController Controller => _controller ??= new TaxiBookingController(_loggerMock.Object,_taxiBookingServiceMock.Object);
        
        /// <summary>
        /// POST /api/book unit tests
        /// </summary>
        [Fact]
        public void PostBookSucceedsWithHttpResponseMessage201Created()
        {
            // Arrange
            var location = new Location()
            {
                Source = new LocationCoordinates()
                {
                    X = 1,
                    Y = 0
                },
                Destination = new LocationCoordinates()
                {
                    X = 1,
                    Y = 1
                }
            };
            _taxiBookingServiceMock.Setup(x => x.BookTaxi(location)).Returns((1,2));
 
            // Act
            var httpResponseMessage = Controller.PostBookTaxi(location);
            var result = httpResponseMessage as ObjectResult;
            
            //Assert
            Assert.NotNull(result);
            Assert.Equal(201,result.StatusCode);
            
            var responseJson = JObject.Parse(result.Value.ToString().Replace('=',':'));
            var carId = (int)responseJson.SelectToken("car_id");
            var totalTime = (int)responseJson.SelectToken("total_time");
                
            Assert.Equal(1,carId);
            Assert.Equal(2,totalTime);
        }
        
        /// <summary>
        /// POST /api/tick unit tests
        /// </summary>
        [Fact]
        public void PostTickSucceedsWithHttpResponseMessage200()
        {
            // Act
            var httpResponseMessage = Controller.PostTick();
            // Assert 
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        }
        
        /// <summary>
        /// POST /api/reset unit tests
        /// </summary>
        [Fact]
        public void ResetSucceedsWithHttpResponseMessage200()
        {
            // Act
            var httpResponseMessage = Controller.Reset(); 
            
            // Assert 
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        }
    }
}