using Common.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class AirportContext : DbContext
    {
        public DbSet<FlightDTO> Flights { get; set; }
        public DbSet<StationDTO> Stations { get; set; }
        public DbSet<FlightsHistoryDTO> FlightsHistory { get; set; }
        public DbSet<FutureDeparturesDTO> FutureDepartures { get; set; }
        public DbSet<FutureArrivalsDTO> FutureArrivals { get; set; }

        public AirportContext() : base("AirportDb2")
        {
            Database.SetInitializer(new DataBaseInitializer());
        }
    }
}

