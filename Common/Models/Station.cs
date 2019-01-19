using Common.Collections.Common.Collections;
using Common.Enums;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public delegate void StationHandler(KeyValuePair<int, string> receiveStation, KeyValuePair<int, string> transferStation);
    public class Station : IStation
    {
        public int Id { get; set; }
        public Flight CurrentFlight { get; set; }
        public StationFlightsType StationFlightsType { get; set; }

        private bool _isAvailable;
        public bool IsAvailable
        {
            get
            {
                return _isAvailable;
            }
            set
            {
                _isAvailable = value;
            }
        }

        private List<Station> _nextStations;
        public List<Station> NextStations
        {
            get
            {
                if (_nextStations == null)
                {
                    _nextStations = new List<Station>();
                }
                return _nextStations;
            }
            set
            {
                _nextStations = value;
            }
        }

        public event StationHandler StationHandlerIds;

        private void HandleStationState(int receiveStationId, string flightId, int transferStationId)
        {
            KeyValuePair<int, string> transferStation = new KeyValuePair<int, string>(transferStationId, null);
            KeyValuePair<int, string> receiveStation;
            if (receiveStationId == 0)
            {
                receiveStation = new KeyValuePair<int, string>(0, null);
            }
            else
            {
                receiveStation = new KeyValuePair<int, string>(receiveStationId, flightId);
            }
            StationHandlerIds.Invoke(receiveStation, transferStation);
        }

        public Station(int id, StationFlightsType stationFlightsType, StationHandler onStationEvent)
        {
            IsAvailable = true;
            StationHandlerIds += onStationEvent;
            StationsCollection.StationsCollectionBag.Add(this);
            Id = id;
            StationFlightsType = stationFlightsType;
            CurrentFlight = null;
        }

        private bool IsLeaveFlight(Flight flight)
        {
            if (this.Id == 4 && flight.FlightType == FlightType.Departure || (this.Id == 6 || this.Id == 7) && flight.FlightType == FlightType.Arrival)
            {
                return true;
            }
            return false;
        }

        private void LeaveAirplane()
        {
            IsAvailable = true;
            CurrentFlight = null;
        }

        private async Task<bool> GetTransferAirplaneTasks(Flight flight)
        {
            bool stationMatch = false;
            bool TaskIsRunning = true;
            while (TaskIsRunning)
            {
                foreach (Station station in this.NextStations)
                {
                    stationMatch = IsStationMatch(flight, station);
                    if (stationMatch && station.IsAvailable)
                    {
                        station.ReceiveAirplane(flight);
                        {
                            LeaveAirplane();
                            HandleStationState(station.Id, flight.Guid, Id);
                            TaskIsRunning = false;
                            break;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private static bool IsStationMatch(Flight flight, Station station)
        {
            if (flight.FlightType == FlightType.Arrival && (station.StationFlightsType == StationFlightsType.Arrivals || station.StationFlightsType == StationFlightsType.DeparturesAndArrivals))
            {
                return true;
            }
            else if (flight.FlightType == FlightType.Departure && (station.StationFlightsType == StationFlightsType.Departures || station.StationFlightsType == StationFlightsType.DeparturesAndArrivals))
            {
                return true;
            }
            return false;
        }

        public async Task ReceiveAirplane(Flight flight)
        {
            CurrentFlight = flight;
            IsAvailable = false;
            bool leaveFlight = IsLeaveFlight(flight);
            if (leaveFlight)
            {
                await flight.DelayAirplane();
                LeaveAirplane();
                HandleStationState(0, null, Id);
                return;
            }
            await flight.DelayAirplane();
            bool transfer = false;
            while (!transfer)
            {
                transfer = this.GetTransferAirplaneTasks(flight).Result;
            }
            LeaveAirplane();
        }
    }
}
