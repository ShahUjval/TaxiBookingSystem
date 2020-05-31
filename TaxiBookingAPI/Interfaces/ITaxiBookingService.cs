using TaxiBookingAPI.Model;

namespace TaxiBookingAPI.Interfaces
{
    /// <summary>
    /// Interface for the Taxi Booking Service API
    /// </summary>
    public interface ITaxiBookingService
    {
        /// <summary>
        /// 
        /// </summary>
        public void ResetTaxiBookingSystem();

        /// <summary>
        /// 
        /// </summary>
        public void IncrementServiceTimeStamp();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public (int, int) BookTaxi(Location location);
    }
}