using Common.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class DataBaseInitializer : CreateDatabaseIfNotExists<AirportContext>
    {
        protected override void Seed(AirportContext context)
        {
            //base.Seed(context);

            FlightDTO flight1 = new FlightDTO
            {
                FlightGuidAndType = "Departure " + Guid.NewGuid().ToString(),
                IsDeparture = true,
                StartTime = DateTime.Now
            };

            FlightDTO flight2 = new FlightDTO
            {
                FlightGuidAndType = "Arrival " + Guid.NewGuid().ToString(),
                IsDeparture = false,
                StartTime = DateTime.Now.AddDays(2)
            };

            FlightDTO flight3 = new FlightDTO
            {
                FlightGuidAndType = "Departure " + Guid.NewGuid().ToString(),
                IsDeparture = true,
                StartTime = DateTime.Now.AddHours(3)
            };

            StationDTO station1 = new StationDTO
            {
                FlightDTO = flight1,
                Name = 1
            };

            StationDTO station2 = new StationDTO
            {
                FlightDTO = null,
                Name = 2
            };

            StationDTO station3 = new StationDTO
            {
                FlightDTO = flight3,
                Name = 3
            };

            StationDTO station4 = new StationDTO
            {
                FlightDTO = null,
                Name = 4
            };

            StationDTO station5 = new StationDTO
            {
                FlightDTO = null,
                Name = 5
            };

            StationDTO station6 = new StationDTO
            {
                FlightDTO = null,
                Name = 6
            };

            StationDTO station7 = new StationDTO
            {
                FlightDTO = null,
                Name = 7
            };

            StationDTO station8 = new StationDTO
            {
                FlightDTO = null,
                Name = 8
            };

            context.Flights.Add(flight1);
            context.Flights.Add(flight2);
            context.Flights.Add(flight3);

            context.Stations.Add(station1);
            context.Stations.Add(station2);
            context.Stations.Add(station3);
            context.Stations.Add(station4);
            context.Stations.Add(station5);
            context.Stations.Add(station6);
            context.Stations.Add(station7);
            context.Stations.Add(station8);

            context.SaveChanges();

            FutureArrivalsDTO futureArrival1 = new FutureArrivalsDTO
            {
                FlightDTO = new FlightDTO
                {
                    FlightGuidAndType = "Arrival " + Guid.NewGuid().ToString(),
                    IsDeparture = false,
                    StartTime = DateTime.Now.AddYears(2)
                },
            };

            FutureArrivalsDTO futureArrival2 = new FutureArrivalsDTO
            {
                FlightDTO = new FlightDTO
                {
                    FlightGuidAndType = "Arrival " + Guid.NewGuid().ToString(),
                    IsDeparture = false,
                    StartTime = DateTime.Now.AddYears(3)
                },
            };

            context.FutureArrivals.Add(futureArrival1);
            context.FutureArrivals.Add(futureArrival2);
            context.SaveChanges();

            FutureDeparturesDTO futureDeparture1 = new FutureDeparturesDTO
            {
                FlightDTO = new FlightDTO
                {
                    FlightGuidAndType = "Departure " + Guid.NewGuid().ToString(),
                    IsDeparture = true,
                    StartTime = DateTime.Now.AddYears(4)
                },
            };

            FutureDeparturesDTO futureDeparture2 = new FutureDeparturesDTO
            {
                FlightDTO = new FlightDTO
                {
                    FlightGuidAndType = "Departure " + Guid.NewGuid().ToString(),
                    IsDeparture = true,
                    StartTime = DateTime.Now.AddYears(7)
                },
            };

            context.FutureDepartures.Add(futureDeparture1);
            context.FutureDepartures.Add(futureDeparture2);
            context.SaveChanges();
        }
    }
}
