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
    public class FlightsRepository
    {
        public IEnumerable<FlightDTO> GetAllFlights()
        {
            using (AirportContext context = new AirportContext())
            {
                return context.Flights.ToList();
            }
        }

        public FlightDTO GetById(int? id)
        {
            using (AirportContext context = new AirportContext())
            {
                return context.Flights.Find(id);
            }
        }

        public int GetIdByGuidAndType(string flightGuidAndType)
        {
            using (AirportContext context = new AirportContext())
            {
                return (int)context.Flights.FirstOrDefault(f => f.FlightGuidAndType == flightGuidAndType).Id;
            }
        }

        public FlightDTO GetFlightByGuidAndType(string flightGuidAndType)
        {
            using (AirportContext context = new AirportContext())
            {
                return context.Flights.FirstOrDefault(f => f.FlightGuidAndType == flightGuidAndType);
            }
        }

        public IEnumerable<FlightDTO> GetAllByType(bool isDeparture)
        {
            using (AirportContext context = new AirportContext())
            {
                var allFlights = context.Flights.ToList();
                var AllByType = (from flight in allFlights where (flight.IsDeparture == isDeparture) select flight).ToList();
                return AllByType;
            }
        }

        public void Add(FlightDTO flight)
        {
            using (AirportContext context = new AirportContext())
            {
                context.Flights.Add(flight);
                context.SaveChanges();
            }
        }

        public void Delete(int? id)
        {
            using (AirportContext context = new AirportContext())
            {
                context.Flights.Remove(context.Flights.Find(id));
                context.SaveChanges();
            }
        }

        public void Update(FlightDTO flight)
        {
            using (AirportContext context = new AirportContext())
            {
                context.Flights.AddOrUpdate(flight);
                context.SaveChanges();
            }
        }
    }
}
