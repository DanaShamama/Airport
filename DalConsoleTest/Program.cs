using Common.DTOs;
using Common.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            StationsRepository stationsRepository = new StationsRepository();
            StationDTO stationDTO = new StationDTO { Name = 1, FlightDTO = null };
            stationsRepository.Add(stationDTO);
            var stations = stationsRepository.GetAllStations();
            foreach (var item in stations)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine(stationDTO.Id);
            Console.ReadLine();
            FutureArrivalsRepository futureArrivalsRepository = new FutureArrivalsRepository();
            string flight1GuidAndType = "Arrival " + Guid.NewGuid().ToString();
            FutureArrivalsDTO futureArrival1 = new FutureArrivalsDTO
            {
                FlightDTO = new FlightDTO
                {
                    FlightGuidAndType = flight1GuidAndType,
                    IsDeparture = false,
                    StartTime = DateTime.Now.AddDays(3)
                }
            };
            futureArrivalsRepository.Add(futureArrival1);
            var arrival1 = futureArrivalsRepository.GetById(3);

            FlightsRepository flightsRepository = new FlightsRepository();

            var flightId = arrival1.FlightDTOId;
            var flight = flightsRepository.GetById((int)flightId);
            Console.WriteLine(stationDTO.Name);
            Console.WriteLine(flight.FlightGuidAndType);

            var stations2 = stationsRepository.GetAllStations();
            foreach (var item in stations2)
            {
                var flightInStationId = item.FlightDTOId;
                string stationToShow = "Station number: " + item.Name;
                if (flightInStationId != null)
                {
                    var flightInStation = flightsRepository.GetById((int)flightInStationId);
                    string flightToShow = ", Flight Id: " + flightInStationId + ", " + flightInStation.FlightGuidAndType;
                    Console.WriteLine(stationToShow + flightToShow);
                }
                else
                {
                    Console.WriteLine(stationToShow + ", There is no flight in the station");
                }

            }

            var flight1Id = flightsRepository.GetIdByGuidAndType(flight1GuidAndType);
            Console.WriteLine(flight1GuidAndType);
            Console.WriteLine(flight1Id);
            Console.ReadLine();
        }
    }
}
