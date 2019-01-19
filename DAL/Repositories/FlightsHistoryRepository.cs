using Common.DTOs;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class FlightsHistoryRepository
    {
        public IEnumerable<FlightsHistoryDTO> GetAllFlights()
        {
            using (AirportContext context = new AirportContext())
            {
                return context.FlightsHistory.ToList();
            }
        }

        public IEnumerable<FlightsHistoryDTO> GetAllFlightsBeforeEnterTime(DateTime enterTime)
        {
            using (AirportContext context = new AirportContext())
            {
                var allFlights = context.FlightsHistory.ToList();
                var flightsBeforeTime = (from flight in allFlights where (flight.EnterStationTime < enterTime) select flight).ToList();
                return flightsBeforeTime;
            }
        }

        public IEnumerable<FlightsHistoryDTO> GetAllFlightsBeforeExitTime(DateTime exitTime)
        {
            using (AirportContext context = new AirportContext())
            {
                var allFlights = context.FlightsHistory.ToList();
                var flightsBeforeTime = (from flight in allFlights where (flight.ExitStationTime < exitTime) select flight).ToList();
                return flightsBeforeTime;
            }
        }

        public IEnumerable<FlightsHistoryDTO> GetAllFlightsAfterEnterTime(DateTime enterTime)
        {
            using (AirportContext context = new AirportContext())
            {
                var allFlights = context.FlightsHistory.ToList();
                var flightsAfterTime = (from flight in allFlights where (flight.EnterStationTime > enterTime) select flight).ToList();
                return flightsAfterTime;
            }
        }

        public IEnumerable<FlightsHistoryDTO> GetAllFlightsAfterExitTime(DateTime exitTime)
        {
            using (AirportContext context = new AirportContext())
            {
                var allFlights = context.FlightsHistory.ToList();
                var flightsAfterTime = (from flight in allFlights where (flight.ExitStationTime > exitTime) select flight).ToList();
                return flightsAfterTime;
            }
        }

        public FlightsHistoryDTO GetById(int id)
        {
            using (AirportContext context = new AirportContext())
            {
                return context.FlightsHistory.Find(id);
            }
        }

        public void Add(FlightsHistoryDTO history)
        {
            using (AirportContext context = new AirportContext())
            {
                context.FlightsHistory.Add(history);
                context.SaveChanges();
            }
        }

        public FlightsHistoryDTO GetHistoryByFlightId(int? flightId)
        {
            using (AirportContext context = new AirportContext())
            {
                return context.FlightsHistory.FirstOrDefault(fh => fh.FlightDTOId == flightId);
            }
        }

        public void Update(FlightsHistoryDTO flightsHistoryDTO)
        {
            using (AirportContext context = new AirportContext())
            {
                context.FlightsHistory.AddOrUpdate(flightsHistoryDTO);
                context.SaveChanges();
            }
        }
    }
}
