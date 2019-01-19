using Common.Collections.Common.Collections;
using Common.Enums;
using Common.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    /// <summary>
    /// a service to approach the station class indirectly 
    /// </summary>
    public class StationService : IStationService
    {
        // a list of all the stations the start the arrival, the ones that begins the track 
        public List<Station> StartingStationsArrival { get; set; }
        // a list of all the stations the start the departure, the ones that begins the track 
        public List<Station> StartingStationsDeparture { get; set; }

        public event StationHandler StationHandlerIds;

        private AirportState _airportState;
        public AirportState AirportState
        {
            get
            {

                return _airportState;

            }
            set
            {

                _airportState = value;

            }
        }

        public StationService()
        {
            InitializeStations();
            SetNextStations();
            StartingStationsArrival = (from station in StationsCollection.StationsCollectionBag where (station.Id == 1) select station).ToList();
            StartingStationsDeparture = (from station in StationsCollection.StationsCollectionBag where (station.Id == 6 || station.Id == 7) select station).ToList();
            AirportState = new AirportState();
        }

        public void RegisterToStationEvent(StationHandler onStationEvent)
        {
            StationHandlerIds += onStationEvent;
        }

        private void SetNextStations()
        {
            var s = (from station in StationsCollection.StationsCollectionBag where (station.Id == 1) select station).FirstOrDefault();
            s.NextStations = (from station in StationsCollection.StationsCollectionBag where (station.Id == 2) select station).ToList();

            s = (from station in StationsCollection.StationsCollectionBag where (station.Id == 2) select station).FirstOrDefault();
            s.NextStations = (from station in StationsCollection.StationsCollectionBag where (station.Id == 3) select station).ToList();

            s = (from station in StationsCollection.StationsCollectionBag where (station.Id == 3) select station).FirstOrDefault();
            s.NextStations = (from station in StationsCollection.StationsCollectionBag where (station.Id == 4) select station).ToList();

            s = (from station in StationsCollection.StationsCollectionBag where (station.Id == 4) select station).FirstOrDefault();
            s.NextStations = (from station in StationsCollection.StationsCollectionBag where (station.Id == 5) select station).ToList();

            s = (from station in StationsCollection.StationsCollectionBag where (station.Id == 5) select station).FirstOrDefault();
            s.NextStations = (from station in StationsCollection.StationsCollectionBag where (station.Id == 6 || station.Id == 7) select station).ToList();

            s = (from station in StationsCollection.StationsCollectionBag where (station.Id == 6) select station).FirstOrDefault();
            s.NextStations = (from station in StationsCollection.StationsCollectionBag where (station.Id == 8) select station).ToList();

            s = (from station in StationsCollection.StationsCollectionBag where (station.Id == 7) select station).FirstOrDefault();
            s.NextStations = (from station in StationsCollection.StationsCollectionBag where (station.Id == 8) select station).ToList();

            s = (from station in StationsCollection.StationsCollectionBag where (station.Id == 8) select station).FirstOrDefault();
            s.NextStations = (from station in StationsCollection.StationsCollectionBag where (station.Id == 4) select station).ToList();
        }

        private void OnStationEvent(KeyValuePair<int, string> receiveStation, KeyValuePair<int, string> transferStation)
        {
            StationHandlerIds.Invoke(receiveStation, transferStation);
        }

        private void InitializeStations()
        {
            Station station1 = new Station(1, StationFlightsType.Arrivals, OnStationEvent);
            Station station2 = new Station(2, StationFlightsType.Arrivals, OnStationEvent);
            Station station3 = new Station(3, StationFlightsType.Arrivals, OnStationEvent);
            Station station4 = new Station(4, StationFlightsType.DeparturesAndArrivals, OnStationEvent);
            Station station5 = new Station(5, StationFlightsType.Arrivals, OnStationEvent);
            Station station6 = new Station(6, StationFlightsType.DeparturesAndArrivals, OnStationEvent);
            Station station7 = new Station(7, StationFlightsType.DeparturesAndArrivals, OnStationEvent);
            Station station8 = new Station(8, StationFlightsType.Departures, OnStationEvent);
        }

        public async Task ReceiveAirplaneToStartingStationsAsync(Flight flight)
        {
            bool taskIsRunning = true;

            if (flight.FlightType == FlightType.Arrival)
            {
                while (taskIsRunning)
                {
                    foreach (var item in StartingStationsArrival)
                    {
                        if (item.IsAvailable)
                        {
                            item.ReceiveAirplane(flight);
                            //if (potentialTask.Result)
                            //{
                            KeyValuePair<int, string> receiveStation = new KeyValuePair<int, string>(item.Id, flight.Guid);
                            KeyValuePair<int, string> transferStation = new KeyValuePair<int, string>(0, null);
                            OnStationEvent(receiveStation, transferStation);
                            taskIsRunning = false;
                            break;
                            //}
                        }
                    }
                }
            }
            if (flight.FlightType == FlightType.Departure)
            {
                while (taskIsRunning)
                {
                    foreach (var item in StartingStationsDeparture)
                    {
                        if (item.IsAvailable)
                        {
                            item.ReceiveAirplane(flight);
                            //if (potentialTask.Result)
                            //{

                            KeyValuePair<int, string> receiveStation = new KeyValuePair<int, string>(item.Id, flight.Guid);
                            KeyValuePair<int, string> transferStation = new KeyValuePair<int, string>(0, null);
                            OnStationEvent(receiveStation, transferStation);
                            taskIsRunning = false;
                            break;
                            //}
                        }
                    }
                }
            }
        }
    }
}
