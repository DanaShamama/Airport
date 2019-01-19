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
    public class FutureArrivalsRepository
    {
        public IEnumerable<FutureArrivalsDTO> GetAllFutureArrivals()
        {
            using (AirportContext context = new AirportContext())
            {
                return context.FutureArrivals.ToList();
            }
        }


        public FutureArrivalsDTO GetById(int? id)
        {
            using (AirportContext context = new AirportContext())
            {
                return context.FutureArrivals.Find(id);
            }
        }

        public FutureArrivalsDTO GetByFlightId(int? flightId)
        {
            using (AirportContext context = new AirportContext())
            {
                var flights = context.FutureArrivals.ToList();
                var Futurflight = (from arrivalToGet in flights where (arrivalToGet.FlightDTOId == flightId) select arrivalToGet).FirstOrDefault();
                return Futurflight;
            }
        }

        public void Add(FutureArrivalsDTO futureArrival)
        {
            using (AirportContext context = new AirportContext())
            {
                context.FutureArrivals.Add(futureArrival);
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (AirportContext context = new AirportContext())
            {
                context.FutureArrivals.Remove(context.FutureArrivals.Find(id));
                context.SaveChanges();
            }
        }

        public void Update(FutureArrivalsDTO futureArrival)
        {
            using (AirportContext context = new AirportContext())
            {
                context.FutureArrivals.AddOrUpdate(futureArrival);
                context.SaveChanges();
            }
        }

        public void UpdateIsEnteredByFlightId(int? flightId)
        {
            using (AirportContext context = new AirportContext())
            {
                var flights = context.FutureArrivals.ToList();
                var futureArrival = (from arrivalToGet in flights where (arrivalToGet.FlightDTOId == flightId) select arrivalToGet).FirstOrDefault();
                FutureArrivalsDTO newFutureArrival = new FutureArrivalsDTO { FlightDTOId = futureArrival.FlightDTOId, IsEntered = true, Id = futureArrival.Id };
                context.FutureArrivals.AddOrUpdate(newFutureArrival);
                context.SaveChanges();
            }
        }
    }
}
