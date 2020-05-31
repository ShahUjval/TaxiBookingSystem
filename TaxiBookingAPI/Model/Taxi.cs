using System.Drawing;

namespace TaxiBookingAPI.Model
{
    /// <summary>
    ///  In Memory Model for the Taxi , Which Holds the three taxi details
    /// </summary>
    public class Taxi
    {
        /// <summary>
        ///  car id either 1 , 2 , 3
        /// </summary>
        public int CarId { get; set; }
        
        /// <summary>
        ///  current location of the car
        /// </summary>
        public LocationCoordinates Location { get; set; }

        /// <summary>
        ///  Is car already booked
        /// </summary>
        public bool IsBooked { get; set; }
        
        /// <summary>
        ///  if c ar is booked , BookedUntilTime will tell you until what time it is booked
        /// </summary>
        public int BookedUntilTime { get; set; }
        
    }
}