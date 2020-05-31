using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TaxiBookingAPI.Model
{
    /// <summary>
    /// Location Model for the JSON 
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Passenger's initial Position
        /// </summary>
        [Required]
        public LocationCoordinates Source { get; set; }
        
        /// <summary>
        /// Passenger's destination Position
        /// </summary>
        [Required]
        public LocationCoordinates Destination { get; set; }
    }

    /// <summary>
    /// Location Coordinates X,Y
    /// </summary>
    public class LocationCoordinates
    {
        /// <summary>
        ///  X coordinate of the Location
        /// </summary>
        [Required]
        public int X { get; set; }
        
        /// <summary>
        /// Y coordinate of the Location
        /// </summary>
        [Required]
        public int Y { get; set; }
    }
}