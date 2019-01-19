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
    public class FutureDeparturesRepository
    {
        public IEnumerable<FutureDeparturesDTO> GetAllFutureDepartures()
        {
            using (AirportContext context = new AirportContext())
            {
                return context.FutureDepartures.ToList();
            }
        }

        public FutureDeparturesDTO GetById(int? id)
        {
            using (AirportContext context = new AirportContext())
            {
                return context.FutureDepartures.Find(id);
            }
        }

        public FutureDeparturesDTO GetByFlightId(int? flightId)
        {
            using (AirportContext context = new AirportContext())
            {
                var flights = context.FutureDepartures.ToList();
                var futureflight = (from departureToGet in flights where (departureToGet.FlightDTOId == flightId) select departureToGet).FirstOrDefault();
                return futureflight;
            }
        }

        public void Add(FutureDeparturesDTO futureDeparture)
        {
            using (AirportContext context = new AirportContext())
            {
                context.FutureDepartures.Add(futureDeparture);
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (AirportContext context = new AirportContext())
            {
                context.FutureDepartures.Remove(context.FutureDepartures.Find(id));
                context.SaveChanges();
            }
        }

        public void Update(FutureDeparturesDTO futureDeparture)
        {
            using (AirportContext context = new AirportContext())
            {
                context.FutureDepartures.AddOrUpdate(futureDeparture);
                context.SaveChanges();
            }
        }

        public void UpdateIsEnteredByFlightId(int? flightId)
        {
            using (AirportContext context = new AirportContext())
            {
                var flights = context.FutureDepartures.ToList();
                var futureDeparture = (from departureToGet in flights where (departureToGet.FlightDTOId == flightId) select departureToGet).FirstOrDefault();
                FutureDeparturesDTO newFutureDeparture = new FutureDeparturesDTO { FlightDTOId = futureDeparture.FlightDTOId, IsEntered = true, Id = futureDeparture.Id };
                context.FutureDepartures.AddOrUpdate(newFutureDeparture);
                context.SaveChanges();
            }
        }
    }
}
