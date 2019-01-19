using AirportClient.Models;
using Common.DTOs;
using Common.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirportClient.ViewModels
{
    public class AirportViewModel : INotifyPropertyChanged
    {
        public List<FlightDTO> FutureDepartures { get; set; }
        public List<FlightDTO> FutureArrivals { get; set; }

        private string _futureDeparturesBoard;
        public string FutureDeparturesBoard
        {
            get
            {
                return _futureDeparturesBoard;
            }
            set
            {
                _futureDeparturesBoard = value;
                OnPropertyChanged("FutureDeparturesBoard");
            }
        }

        private string _futureArrivalsBoard;
        public string FutureArrivalsBoard
        {
            get
            {
                return _futureArrivalsBoard;
            }
            set
            {
                _futureArrivalsBoard = value;
                OnPropertyChanged("FutureArrivalsBoard");
            }
        }

        private AirportStateDTO _currentAirportState;
        public AirportStateDTO CurrentAirportState
        {
            get
            {
                if (_currentAirportState == null)
                {
                    _currentAirportState = new AirportStateDTO();
                }
                return _currentAirportState;

            }
            set
            {
                _currentAirportState = value;
                OnPropertyChanged("CurrentAirportState");
            }
        }

        private string _currentAirportStateBoard;
        public string CurrentAirportStateBoard
        {
            get
            {
                return _currentAirportStateBoard;
            }
            set
            {
                _currentAirportStateBoard = value;
                OnPropertyChanged("CurrentAirportStateBoard");
            }
        }

        private ObservableCollection<ViewStation> _stations;
        public ObservableCollection<ViewStation> Stations
        {
            get
            {
                return _stations;
            }
            set
            {
                _stations = value;
                OnPropertyChanged("Stations");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// a handler to get the name of the property that changed in the view
        /// </summary>
        /// <param name="propertyName">string, the name of the property that have changed</param>
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static HubConnection _hubConnection = null;
        public static HubConnection HubConnection
        {
            get
            {
                if (_hubConnection == null)
                {
                    _hubConnection = new HubConnection("http://localhost:52561/");
                }
                return _hubConnection;
            }
            set
            {
                _hubConnection = value;
            }
        }

        private static IHubProxy _airportProxy = null;
        public static IHubProxy AirportProxy
        {
            get
            {
                if (_airportProxy == null)
                {
                    _airportProxy = HubConnection.CreateHubProxy("AirportHub");
                }
                return _airportProxy;
            }
            set
            {
                _airportProxy = value;
            }
        }

        public AirportViewModel()
        {
            InitalizeStations();
            FutureDepartures = new List<FlightDTO>();
            FutureArrivals = new List<FlightDTO>();
            AirportProxy.On("UpdateAirportState", (AirportStateDTO airportState) =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    UpdateStateBoard(airportState);
                    UpdateFutureFlightsBoards(airportState);
                }));
            });

            AirportProxy.On("UpdateFutureFlights", (FlightDTO flightDTO) =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (flightDTO.IsDeparture)
                    {
                        WriteToFutureDeparturesBoard(flightDTO);
                    }
                    else
                    {
                        WriteToFutureArrivalsBoard(flightDTO);
                    }
                }));
            });

            HubConnection.Start().Wait();

        }

        private void WriteToFutureArrivalsBoard(FlightDTO flightDTO)
        {
            FutureArrivals.Add(flightDTO);
            StringBuilder sb = new StringBuilder("Future Arrivals:\n");
            foreach (var item in FutureArrivals)
            {
                string flight;
                if (!item.IsDeparture)
                {
                    flight = "Flight: " + item.FlightGuidAndType + " Start time: " + item.StartTime;
                    sb.Append(flight + "\n");
                }
            }
            if (FutureArrivals.Count == 0)
            {
                sb.Append("No future arrivals at the moment\n");
            }
            FutureArrivalsBoard = sb.ToString();
        }

        private void UpdateFutureFlightsBoards(AirportStateDTO airportState)
        {
            foreach (var item in airportState.CurrentStationsState)
            {
                foreach (var departure in FutureDepartures)
                {
                    if (item.Value == departure.FlightGuidAndType)
                    {
                        FutureDepartures.Remove(departure);
                        break;
                    }
                }
                foreach (var departure in FutureArrivals)
                {
                    if (item.Value == departure.FlightGuidAndType)
                    {
                        FutureArrivals.Remove(departure);
                        break;
                    }
                }
            }
        }

        private void WriteToFutureDeparturesBoard(FlightDTO flightDTO)
        {
            FutureDepartures.Add(flightDTO);
            StringBuilder sb = new StringBuilder("Future Departures:\n");
            foreach (var item in FutureDepartures)
            {
                string flight;
                if (item.IsDeparture)
                {
                    flight = "Flight: " + item.FlightGuidAndType + " Start time: " + item.StartTime;
                    sb.Append(flight + "\n");
                }
            }
            if (FutureDepartures.Count == 0)
            {
                sb.Append("No future departures at the moment\n");
            }
            FutureDeparturesBoard = sb.ToString();
        }

        private void UpdateStateBoard(AirportStateDTO airportState)
        {
            SetStationsVisibility(airportState);
            WriteFlightsToTheBoard();
        }

        private void WriteFlightsToTheBoard()
        {
            StringBuilder sb = new StringBuilder("Current airport state: \n");
            foreach (var item in CurrentAirportState.CurrentStationsState)
            {

                string flight;
                if (item.Value == null)
                {
                    flight = " No airplane in the station\n";
                }
                else
                {
                    flight = " Current flight Id: " + item.Value + "\n";
                }
                sb.Append("Station number " + item.Key + ": " + flight);
            }
            CurrentAirportStateBoard = sb.ToString();
        }

        private void SetStationsVisibility(AirportStateDTO airportState)
        {
            CurrentAirportState = airportState;
            foreach (var item in Stations)
            {
                foreach (var stationState in CurrentAirportState.CurrentStationsState)
                {
                    if (item.Id == stationState.Key)
                    {
                        if (stationState.Value != null)
                        {
                            item.IsAvailable = false;
                            item.Visibility = Visibility.Visible;
                            item.CurrentFlightId = stationState.Value;
                        }
                        else
                        {
                            item.IsAvailable = true;
                            item.Visibility = Visibility.Collapsed;
                            item.CurrentFlightId = "";
                        }
                    }
                }
            }
        }

        private void InitalizeStations()
        {
            Stations = new ObservableCollection<ViewStation>();
            Stations.Add(new ViewStation { Id = 1 });
            Stations.Add(new ViewStation { Id = 2 });
            Stations.Add(new ViewStation { Id = 3 });
            Stations.Add(new ViewStation { Id = 4 });
            Stations.Add(new ViewStation { Id = 5 });
            Stations.Add(new ViewStation { Id = 6 });
            Stations.Add(new ViewStation { Id = 7 });
            Stations.Add(new ViewStation { Id = 8 });
        }
    }
}
