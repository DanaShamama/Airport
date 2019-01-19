using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Common.Interfaces;
using Common.Models;
using Common.DTOs;
using Common.Services;
using Common.Convertors;
using System.Threading.Tasks;

namespace AirportServerBL
{
    public class AirportManager : IAirportManager
    {
        private SortedList<DateTime, FlightDTO> _airplanesList;
        private Timer _timer;
        private IStationService _stationService;
        private static readonly object lockObj = new object();

        private static AirportState _airportState;
        public static AirportState AirportState
        {
            get
            {
                lock (lockObj)
                {
                    if (_airportState == null)
                    {
                        _airportState = new AirportState();
                    }
                    return _airportState;
                }
            }
            set
            {
                lock (lockObj)
                {
                    value = _airportState;
                }
            }
        }
        public event AirportStateHandler AirportStateHandler;
        public event FutureFlightsHandler FutureFlightsHandler;
        public FutureDeparturesRepository FutureDeparturesRepository { get; set; }
        public FutureArrivalsRepository FutureArrivalsRepository { get; set; }
        public FlightsRepository FlightsRepository { get; set; }
        public StationsRepository StationsRepository { get; set; }
        public FlightsHistoryRepository FlightsHistoryRepository { get; set; }

        public AirportManager()
        {
            _stationService = new StationService();
            _stationService.RegisterToStationEvent(UpdateAirportState);
            _airplanesList = new SortedList<DateTime, FlightDTO>();
            FutureDeparturesRepository = new FutureDeparturesRepository();
            FutureArrivalsRepository = new FutureArrivalsRepository();
            FlightsRepository = new FlightsRepository();
            StationsRepository = new StationsRepository();
            FlightsHistoryRepository = new FlightsHistoryRepository();
            TimerInitialize();
        }

        public void RegisterAirportStateEvent(AirportStateHandler onStateEvent)
        {
            AirportStateHandler += onStateEvent;
        }

        public void RegisterFutureFlightsEvent(FutureFlightsHandler onFutureFlightEvent)
        {
            FutureFlightsHandler += onFutureFlightEvent;
        }

        private void TimerInitialize()
        {
            _timer = new Timer();
            _timer.Elapsed += _timer_Elapsed;
        }

        public void UpdateAirportState(KeyValuePair<int, string> receiveStation, KeyValuePair<int, string> transferStation)
        {
            UpdateStationsDatabase(receiveStation, transferStation);
            UpdateStationState(receiveStation);
            UpdateStationState(transferStation);
            AirportState.CurrentTime = DateTime.Now;
            OnAirportEvent();         
        }

        private void UpdateStationsDatabase(KeyValuePair<int, string> receiveStation, KeyValuePair<int, string> transferStation)
        {
            if (receiveStation.Key != 0)
            {
                StationDTO stationToUpdate = StationsRepository.GetByName(receiveStation.Key);
                FlightDTO inStationFlightDTO = FlightsRepository.GetFlightByGuidAndType(receiveStation.Value);
                stationToUpdate.FlightDTO = inStationFlightDTO;
                stationToUpdate.FlightDTOId = inStationFlightDTO.Id; 
                StationsRepository.Update(stationToUpdate);
            }
            if (transferStation.Key != 0)
            {
                StationDTO stationToUpdate = StationsRepository.GetByName(transferStation.Key);
                var ExitFlightId = stationToUpdate.FlightDTOId;
                stationToUpdate.FlightDTO = null;
                stationToUpdate.FlightDTOId = null;
                StationsRepository.Update(stationToUpdate);
            }
        }

        private void UpdateStationState(KeyValuePair<int, string> station)
        {
            if (station.Key != 0)
            {
                KeyValuePair<int, string> itemToReplace = new KeyValuePair<int, string>(station.Key, station.Value);
                foreach (var item in AirportState.CurrentStationsState)
                {
                    if (item.Key == station.Key)
                    {
                        AirportState.CurrentStationsState.Remove(item);
                        AirportState.CurrentStationsState.Add(itemToReplace);
                        break;
                    }
                }
            }
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            FlightDTO nextFlight = _airplanesList.Values[0];
            _stationService.ReceiveAirplaneToStartingStationsAsync(FromDtoToModel.FromAirplaneDtoToAirplaneModel(nextFlight));
            _airplanesList.RemoveAt(0);
            UpdateFutureFlightsIsEnterd(nextFlight);
            KillTimer();
            TimerInitialize();
            if (_airplanesList.Count() != 0)
            {
                SetInterval(nextFlight);
            }
        }

        private void UpdateFutureFlightsIsEnterd(FlightDTO nextFlight)
        {
            if (nextFlight.IsDeparture)
            {
                FutureDeparturesRepository.UpdateIsEnteredByFlightId(nextFlight.Id);
            }
            else
            {
                FutureArrivalsRepository.UpdateIsEnteredByFlightId(nextFlight.Id);
            }
        }

        private void KillTimer()
        {
            _timer.Stop();
            _timer.Elapsed -= _timer_Elapsed;
            _timer.Dispose();
        }

        public void ReceiveAirplaneFromControler(FlightDTO flight)
        {
            OnFutureFlightEvent(flight);
            UpdateDatabaseFutureFlights(flight);

            if (_airplanesList.Count != 0)
            {
                var currentFirstTime = _airplanesList.Values[0].StartTime;
                _airplanesList.Add(flight.StartTime, flight);
                if (flight.StartTime < currentFirstTime)
                {
                    SetInterval(flight);
                }
            }
            else
            {
                _airplanesList.Add(flight.StartTime, flight);
                SetInterval(flight);
            }
        }

        private async Task UpdateDatabaseFutureFlights(FlightDTO flight)
        {
            if (flight.IsDeparture)
            {
                FutureDeparturesDTO futureDeparturesDTO = new FutureDeparturesDTO { FlightDTO = flight };
                await Task.Run(() => FutureDeparturesRepository.Add(futureDeparturesDTO));
            }
            else
            {
                FutureArrivalsDTO futureArrivalsDTO = new FutureArrivalsDTO { FlightDTO = flight };
                await Task.Run(() => FutureArrivalsRepository.Add(futureArrivalsDTO));
            }
        }

        private void SetInterval(FlightDTO flight)
        {
            var newTime = flight.StartTime.Subtract(DateTime.Now).TotalMilliseconds;
            _timer.Interval = newTime > 0 ? newTime : 1;
            _timer.Start();
        }

        public void OnAirportEvent()
        {
            AirportStateHandler.Invoke(AirportState);
        }

        public void OnFutureFlightEvent(FlightDTO flightDTO)
        {
            FutureFlightsHandler.Invoke(flightDTO);
        }
    }
}
