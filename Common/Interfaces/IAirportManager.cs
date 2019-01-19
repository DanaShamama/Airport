
using Common.DTOs;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public delegate void AirportStateHandler(AirportState airportState);
    public delegate void FutureFlightsHandler(FlightDTO flightDTO);

    public interface IAirportManager
    {
        void ReceiveAirplaneFromControler(FlightDTO flight);
        void RegisterAirportStateEvent(AirportStateHandler airportStateHandler);
        void RegisterFutureFlightsEvent(FutureFlightsHandler futureFlightsHandler);
    }
}
