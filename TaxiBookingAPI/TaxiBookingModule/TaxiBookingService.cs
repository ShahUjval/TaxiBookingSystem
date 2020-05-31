using System;
using System.Collections.Generic;
using System.Linq;
using TaxiBookingAPI.Interfaces;
using TaxiBookingAPI.Model;

namespace TaxiBookingAPI.TaxiBookingModule
{
    /// <summary>
    /// Taxi Booking Service
    /// </summary>
    public class TaxiBookingService : ITaxiBookingService
    {
        /// <summary>
        /// Service Time Stamp
        /// </summary>
        private int _serviceTimeStamp = 0;
        
        /// <summary>
        ///  Taxi Count : 3
        /// </summary>
        private const int TAXI_COUNT = 3;
        
        /// <summary>
        /// collection of taxis with all the information
        /// </summary>
        private Taxi[] taxis;
        
        /// <summary>
        ///  TaxiBookingService Constructor
        /// </summary>
        public TaxiBookingService()
        {
            taxis = new Taxi[TAXI_COUNT];
            InitialiseOrReset();
        }

        /// <summary>
        ///  Initialise Or Reset 
        /// </summary>
        private void InitialiseOrReset()
        {
            _serviceTimeStamp = 0;
            for (var i = 0; i < taxis.Length; i++)
            {
                taxis[i] = new Taxi
                {
                    CarId = i + 1,
                    Location = new LocationCoordinates() {X = 0 , Y = 0},
                    IsBooked = false,
                    BookedUntilTime = -1
                };
            }
        }

        /// <summary>
        /// Reset taxi Booking System
        /// </summary>
        public void ResetTaxiBookingSystem()
        {
            InitialiseOrReset();
        }

        /// <summary>
        ///  Increment Service Time Stamp
        /// </summary>
        public void IncrementServiceTimeStamp()
        {
            _serviceTimeStamp += 1;

            // Let's Check if the car has reached it's destination , 
            //  if (reached)  :  make the car free , so it can be booked again
            foreach (var taxi in taxis)
            {
                if (taxi.BookedUntilTime + 1 == _serviceTimeStamp)
                    taxi.IsBooked = false;
            }
        }

        /// <summary>
        ///  Main Method which Books a Taxi
        /// </summary>
        public (int,int) BookTaxi(Location location)
        {
            //time need to take customer from source to destination
            var timeRequiredCustomer = TimeTakenFromXtoY(location.Source, location.Destination);

            //time needed for the nearest car to reach customer's source
            var (carId, timeRequiredToReachCustomer) = FindNearestTaxiToCustomer(location.Source);

            // book the taxi and send the details back
            foreach (var taxi in taxis)
            {
                if (taxi.CarId == carId)
                {
                    taxi.IsBooked = true;
                    //place the car at dest coordinates since it is booked anyway
                    taxi.Location.X = location.Destination.X;
                    taxi.Location.Y = location.Destination.Y;
                    taxi.BookedUntilTime = timeRequiredCustomer + timeRequiredToReachCustomer + _serviceTimeStamp;
                    return (carId, (timeRequiredCustomer + timeRequiredToReachCustomer));
                }
            }

            return (-1, -1);
        }

        /// <summary>
        /// Calculates the Manhattan distance between two points
        ///  x = (a,b)  and   y = (c,d)
        ///  Manhattan Distance is calculated  = |a - c| + |b - d| 
        /// </summary>
        /// <param name="locationSource">Source Location</param>
        /// <param name="locationDestination">Destination Location</param>
        /// <returns></returns>
        private int TimeTakenFromXtoY(LocationCoordinates locationSource, LocationCoordinates locationDestination)
        {
            return Math.Abs(locationDestination.X - locationSource.X) +
                   Math.Abs(locationDestination.Y - locationSource.Y);

        }


        /// <summary>
        ///  This Function returns the nearest taxi with the smallest id , given the customer's current position and the
        ///  position of all taxi cars that are available
        /// </summary>
        /// <param name="locationSource"> location source of the Customer's Current location</param>
        /// <returns>nearest car_id with total time required to reach to customer</returns>
        private (int, int) FindNearestTaxiToCustomer(LocationCoordinates locationSource)
        {
            // Dictionary to hold the car ID and Time to travel to customer 
            //
            // car ID     | Time required to reach Customer 
            // ---------------------------------------------
            //   1        |       3 
            //   2        |       6 
            //   3        |       8
            
            var timeRequiredToTravelToCustomerWithCarIdDict = new Dictionary<int,int>();
            
            foreach (var taxi in taxis)
            {
                if (!taxi.IsBooked)
                {
                    var timeToTravelToCustomerForTaxi = TimeTakenFromXtoY(taxi.Location, locationSource);
                    timeRequiredToTravelToCustomerWithCarIdDict.Add(taxi.CarId,timeToTravelToCustomerForTaxi);
                }
            }
            
            if (timeRequiredToTravelToCustomerWithCarIdDict.Count > 0)
            {
                var query = timeRequiredToTravelToCustomerWithCarIdDict.GroupBy(x => x.Value,
                    (k, g) => new {
                        Min = g.Min(x => x.Key),
                        Value = k
                    });
                return (timeRequiredToTravelToCustomerWithCarIdDict.FirstOrDefault().Key,
                    timeRequiredToTravelToCustomerWithCarIdDict.FirstOrDefault().Value);
            }

            return (-1, -1);
        }

    }
}