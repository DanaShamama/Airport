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
    public class StationsRepository
    {
        public IEnumerable<StationDTO> GetAllStations()
        {
            using (AirportContext context = new AirportContext())
            {
                return context.Stations.ToList();
            }
        }

        public StationDTO GetById(int? id)
        {
            using (AirportContext context = new AirportContext())
            {
                return context.Stations.Find(id);
            }
        }

        public StationDTO GetByName(int name)
        {
            using (AirportContext context = new AirportContext())
            {
                return (from station in context.Stations where (station.Name == name) select station).FirstOrDefault();
            }
        }

        public void Add(StationDTO station)
        {
            using (AirportContext context = new AirportContext())
            {
                context.Stations.Add(station);
                context.SaveChanges();
            }
        }

        public void Delete(int? id)
        {
            using (AirportContext context = new AirportContext())
            {
                context.Stations.Remove(context.Stations.Find(id));
                context.SaveChanges();
            }
        }

        public void Update(StationDTO station)
        {
            using (AirportContext context = new AirportContext())
            {
                context.Stations.AddOrUpdate(station);
                context.SaveChanges();
            }
        }
    }
}
