using Common.DTOs;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace FlightsSimulator
{   /// <summary>
    /// class simulator generates new flights and send them via the controller to the AirportServerBL
    /// </summary>
    class Simulator
    {
        // the timer for sending a new flight every elapsed time  
        public static System.Timers.Timer Timer { get; set; }
        public static Random Rnd;
        // object to be used by the lock
        private static readonly Object lockObj = new object();

        static void Main(string[] args)
        {
            Rnd = new Random();
            InitalizeTimer();

            Console.Read();
        }

        /// <summary>
        /// initilize the timer at the first time and each time after sending a flight
        /// </summary>
        private static void InitalizeTimer()
        {
            lock (lockObj)
            {
                Timer = new System.Timers.Timer();
                Timer.Elapsed += Timer_Elapsed;
                var time = Rnd.Next(1000, 5000);
                Timer.Interval = time;
                Timer.Start();
            }

        }

        /// <summary>
        /// an event that is raised every time the timer elapsed - it send a new flight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string flightGuid = Guid.NewGuid().ToString();
            flightGuid = flightGuid.Substring(0, 8);
            bool isDeparture = GetFlightType();
            string flightGuidAndType = null;
            if (isDeparture)
            {
                flightGuidAndType = "Departure " + flightGuid;
            }
            else
            {
                flightGuidAndType = "Arrival " + flightGuid;
            }
            DateTime startTime = GetStartTime();
            FlightDTO newAirplane = new FlightDTO { FlightGuidAndType = flightGuidAndType, StartTime = startTime, IsDeparture = isDeparture };
            SendAirplaneToController(newAirplane);
        }

        private static void SendAirplaneToController(FlightDTO newAirplane)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var httpResponse = httpClient.PostAsJsonAsync("http://localhost:52561/api/Flights", newAirplane).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Flight Type Is Departure: " + newAirplane.IsDeparture + " flight Guid: " + newAirplane.FlightGuidAndType + " Time: " + DateTime.Now);
                }
                else
                {
                    Console.WriteLine("An error occurred: " + httpResponse.ReasonPhrase + " Time: " + DateTime.Now);
                }
            }
        }      

        /// <summary>
        /// get a new start time for the airplane that will be sent to the controller
        /// </summary>
        /// <returns>date time - a new random future time</returns>
        private static DateTime GetStartTime()
        {
            return DateTime.Now.AddMilliseconds(Rnd.Next(1000, 5000));
        }

        /// <summary>
        /// determine if the flight type will be a departure (2 to 3 chance) or an arrival (1 to 3 chance)
        /// </summary>
        /// <returns> random bool -  true or false</returns>
        private static bool GetFlightType()
        {
            int intToBool = Rnd.Next(0, 3);
            if (intToBool == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
